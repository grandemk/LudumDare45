using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    void Start()
    {
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        transform.Translate(Time.deltaTime * speed * direction);
    }
}
