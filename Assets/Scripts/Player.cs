using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private float normal_speed = 5f;

    public int hunger_meter = 0;
    public int max_hunger_meter = 10000;

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

    private Vector3 scale;

    [SerializeField]
    bool god_mode = false;


    private void Start()
    {
        grace_period_end = Time.time + grace_period;
        next_consume_time = Time.time;
        scale = transform.localScale;
    }

    void Update()
    {
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
        ui.Show("You Win", "success");
        Destroy(this.gameObject);
    }

    void PlayerDied(string message)
    {
        Debug.Log(message);
        var ui = GetComponent<PlayerAnnouncement>();
        ui.Show("You Died", "failure");
        Destroy(this.gameObject);
    }

    void Hunger()
    {
        hunger_meter--;

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
                PlayerDied("Player Died. Too Hungry");
            }
        }


        if (hunger_meter > max_hunger_meter)
        {
            PlayerWin("Player Won. Satisfied his hunger");
        }
    }

    public void Feed(Food food)
    {
        var audio_source = GetComponent<AudioSource>();
        if (audio_source != null)
            audio_source.Play(1);
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
        }

        if (Time.time > consume_effect_end)
            speed = normal_speed;

        transform.Translate(Time.deltaTime * speed * direction);

        if(player_is_out_of_bound && !god_mode)
            PlayerDied("Player Died. Got Caught Outside the Camera");
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

