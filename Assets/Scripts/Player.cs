using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private int hunger_meter = 0;

    [SerializeField]
    private float grace_period = 5;
    private float grace_period_end;
    private bool grace_has_ended;

    private void Start()
    {
        grace_period_end = Time.time + grace_period;
    }

    void Update()
    {
        Movement();
        Hunger();
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

    void Movement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        transform.Translate(Time.deltaTime * speed * direction);
    }
}
