using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Sprite death_sprite;
    public Sprite win_sprite;
    public Sprite idle1_sprite;
    public Sprite idle2_sprite;
    public Sprite consume_sprite;

    float anim_idle_change_time;
    bool anim_toggle = false;
    bool player_dead = false;

    void UpdateSprite(Sprite new_sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = new_sprite; 
    }

    [SerializeField]
    private float speed = 5f;
    private float normal_speed = 5f;

    [SerializeField]
    private int hunger_meter = 0;
    public int max_hunger_meter = 10000;
    private int previous_hunger_meter = 0;
    private Queue<int> hunger_meter_diffs = new Queue<int>();
    private Queue<int> hunger_meter_decrs = new Queue<int>();

    [SerializeField]
    private float grace_period = 5;
    private float grace_period_end;
    private bool grace_has_ended;

    private float consume_cooldown = 1f;
    private float next_consume_time;
    private float consume_effect_time = 0.5f;
    private float consume_effect_end;
    private bool can_consume = false;
    private float consume_speed = 10f;
    private bool player_is_out_of_bound = false;
    private float signal_life_time;

    private Vector3 scale;

    [SerializeField]
    bool god_mode = false;

    private void Start()
    {
        var blood = GameObject.FindGameObjectWithTag("Blood");
        signal_life_time = Time.time;
        grace_period_end = Time.time + grace_period;
        next_consume_time = Time.time;
        scale = transform.localScale;

        var image = blood.GetComponent<Image>();
        var c = image.color;
        c.a = 0f;
        image.color = c;
    }

    void Update()
    {
        if (player_dead)
            return;
        Movement();
        Hunger();
    }

    void OnBecameInvisible()
    {
        player_is_out_of_bound = true;
    }

    void PlayerWin(string message)
    {
        Debug.Log(message);
        var ui = GetComponent<PlayerAnnouncement>();
        ui.Show(message, "success");
        UpdateSprite(win_sprite);
        Destroy(this.gameObject, 3f);
    }

    void PlayerDied(string message)
    {
        Debug.Log(message);
        player_dead = true;
        var ui = GetComponent<PlayerAnnouncement>();
        ui.Show(message, "failure");
        UpdateSprite(death_sprite);
        Destroy(this.gameObject, 3f);
    }

    public int IncrementHungerMeter(int modifier)
    {
        hunger_meter += modifier;
        if (hunger_meter_decrs.Count > 120f)
            hunger_meter_decrs.Dequeue();
        hunger_meter_decrs.Enqueue(modifier);
        return hunger_meter;
    }

    public int GetHungerMeter()
    {
        return hunger_meter;
    }

    public int DecrementHungerMeter(int modifier)
    {
        hunger_meter -= modifier;
        if (hunger_meter_decrs.Count > 120f)
            hunger_meter_decrs.Dequeue();
        hunger_meter_decrs.Enqueue(modifier);
        return hunger_meter;
    }

    // on last 1s
    public float GetHungerTrend()
    {
        float diff_average = 0;
        foreach(var diff in hunger_meter_diffs)
        {
            diff_average += diff;
        }
        var count = hunger_meter_diffs.Count;
        if (count == 0)
            return 0;
        diff_average /= (float)hunger_meter_diffs.Count;
        return diff_average;
    }

    public float GetHungerDecr()
    {
        float decr_sum = 0;
        foreach(var decr in hunger_meter_decrs)
            decr_sum += decr;

        return decr_sum;
    }

    private void UpdateHungerTrend()
    {
        hunger_meter_diffs.Enqueue(hunger_meter - previous_hunger_meter);
        if (hunger_meter_diffs.Count > 60)
            hunger_meter_diffs.Dequeue();
        previous_hunger_meter = hunger_meter;
    }

    void Hunger()
    {
        DecrementHungerMeter(1);
        UpdateHungerTrend();

        if (hunger_meter > 1000)
        {
            if (can_consume == false)
            {
                Debug.Log("Player can now consume");
                var ui = GetComponent<PlayerAnnouncement>();
                ui.ShowFor("Right Button to Consume", "info", 4f);
            }
            can_consume = true;
        }

        if (god_mode)
            return;

        if (Time.time > grace_period_end)
        {
            if(!grace_has_ended)
                Debug.Log("Grace Period End");

            grace_has_ended = true;

            if (hunger_meter < 0)
            {
                PlayerDied("You died from Hunger");
            }
        }


        if (hunger_meter > max_hunger_meter)
        {
            PlayerWin("You Won! Finally Satiated !");
        }

        Debug.Log("trend: " + GetHungerTrend() + ", decr: " + GetHungerDecr() + ", hunger: " + GetHungerMeter());
        bool should_signal_life = GetHungerMeter() < 300;
        if (should_signal_life && Time.time > signal_life_time)
        {
            signal_life_time += 5f;
            var ui = GetComponent<PlayerAnnouncement>();
            ui.ShowFor("So Hungry...", "info", 1f);
        }

        if(GetHungerDecr() > 800 || GetHungerMeter() < 300)
        {
            var blood = GameObject.FindGameObjectWithTag("Blood");
            if(blood != null)
            {
                var image = blood.GetComponent<Image>();
                var c = image.color;
                c.a = 0.5f;
                image.color = c;
            }
        }
        else
        {
            var blood = GameObject.FindGameObjectWithTag("Blood");
            if(blood != null)
            {
                var image = blood.GetComponent<Image>();
                var c = image.color;
                c.a = 0f;
                image.color = c;
            }

        }
    }

    public void Feed(Food food)
    {
        var audio_source = GetComponent<AudioSource>();
        if (audio_source != null)
            audio_source.Play();
        hunger_meter += food.satiation_value;
    }

    void Movement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        if (Input.GetMouseButtonDown(1) && can_consume && next_consume_time < Time.time)
        {
            next_consume_time = Time.time + consume_cooldown;
            speed = consume_speed;
            consume_effect_end = Time.time + consume_effect_time;
            StartCoroutine(ScaleControl(2, consume_effect_time));
            UpdateSprite(consume_sprite);
        }

        if (Time.time > consume_effect_end)
        {
            speed = normal_speed;
            UpdateSprite(idle1_sprite);
            if (Time.time > anim_idle_change_time)
            {
                anim_idle_change_time += 0.5f;
                anim_toggle = !anim_toggle;
                if(anim_toggle)
                    UpdateSprite(idle2_sprite);
                else
                    UpdateSprite(idle1_sprite);
            }
        }

        transform.Translate(Time.deltaTime * speed * direction);

        if (player_is_out_of_bound && !god_mode)
            PlayerDied("You Died in the Abyss...");
    }

    IEnumerator ScaleControl(float final_scale, float consume_effect_time)
    {
        var starting_local_scale = transform.localScale;
        var num_steps = 20;
        var step_time = consume_effect_time / (2f * num_steps);
        var start_scale = 1f;

        var cur_scale = 1f;
        for (int cur_step = 0; cur_step < num_steps; ++cur_step)
        {
            cur_scale = start_scale + final_scale * (cur_step + 1) / num_steps;
            transform.localScale = starting_local_scale * cur_scale;
            yield return new WaitForSeconds(step_time);
        }

        for (int cur_step = 0; cur_step < num_steps; ++cur_step)
        {
            cur_scale = start_scale + final_scale * (num_steps - cur_step - 1) / num_steps;
            transform.localScale = starting_local_scale * cur_scale;
            yield return new WaitForSeconds(step_time);
        }
    }
}

