using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    public int hunger_meter = 0;
    public int max_hunger_meter = 10000;

    [SerializeField]
    private float grace_period = 5;
    private float grace_period_end;
    private bool grace_has_ended;

    [SerializeField]
    private float border_x = 13;

    private void Start()
    {
        grace_period_end = Time.time + grace_period;
    }

    void Update()
    {
        Movement();
        Hunger();
    }

    void OnBecameInvisible()
    {
        Debug.Log("Player Died. Got Caught Outside the Camera");
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
                Debug.Log("Player Died. Too Hungry");
                Destroy(this.gameObject);
            }
        }

    }

    public void Feed(BasicFood food)
    {
        hunger_meter += food.satiation_value;
    }

    void Movement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);
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
