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

    [SerializeField]
    private float border_x = 9.5f;

    private float dash_cooldown = 1f;
    private float next_dash_time;
    private float dash_effect_time = 0.5f;
    private float dash_effect_end;
    private bool can_dash = false;
    private float dash_speed = 10f;


    private void Start()
    {
        grace_period_end = Time.time + grace_period;
        next_dash_time = Time.time;
    }

    void Update()
    {
        Movement();
        Hunger();
    }

    void OnBecameInvisible()
    {
        PlayerDied("Player Died. Got Caught Outside the Camera");
    }

    void PlayerDied(string message)
    {
        Debug.Log(message);
        var ui = GetComponent<PlayerAnnouncement>();
        ui.Show("You died", "failure");
        Destroy(this.gameObject);
    }

    void Hunger()
    {
        hunger_meter--;

        if (Time.time > grace_period_end)
        {
            if(!grace_has_ended)
                Debug.Log("Grace Period End");

            grace_has_ended = true;

            if (hunger_meter < 0)
            {
                PlayerDied("Player Died. Too Hungry");
            }
            else if (hunger_meter > 1000)
            {
                if (can_dash == false)
                {
                    Debug.Log("Player can now dash");
                    var ui = GetComponent<PlayerAnnouncement>();
                    ui.ShowFor("Use Q to dash", "info", 4f);
                }
                can_dash = true;
            }
        }
    }

    public void Feed(Food food)
    {
        hunger_meter += food.satiation_value;
    }

    void Movement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        if (Input.GetKeyDown(KeyCode.Q) && can_dash && next_dash_time < Time.time)
        {
            next_dash_time = Time.time + dash_cooldown;
            speed = dash_speed;
            dash_effect_end = Time.time + dash_effect_time;
        }

        if (Time.time > dash_effect_end)
            speed = normal_speed;

        transform.Translate(Time.deltaTime * speed * direction);
        transform.position = WrapX(transform.position, -border_x, border_x);
    }

    Vector3 WrapX(Vector3 v, float a, float b)
    {
        if (v.x > b)
        {
            return new Vector3(a, transform.position.y, 0);
        }
        else if (v.x <= a)
        {
            return new Vector3(b, transform.position.y, 0);
        }
        else
        {
            return v;
        }
    }
}
