using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 0.01f;
    private float min_speed = 0.01f;

    private float speed_update_delay = 10f;
    private float speed_update_scale = 0.005f;
    private float next_speed_update_time;


    [SerializeField]
    private float end_of_world_y = 30f;
    private float end_of_world_x_1 = 25.1f;
    private float end_of_world_x_2 = 52.2f;

    public Player player;

    /* v in [a, b[ */
    private bool IsBetween(float v, float a, float b)
    {
        return v >= a && v < b;
    }

    private void Start()
    {
        next_speed_update_time = Time.time + speed_update_delay;
    }

    void Update()
    {
        if (player == null)
            return;

        if(player.player_finished)
            return;

        if (Time.time > next_speed_update_time)
        {
            next_speed_update_time += speed_update_delay;
            min_speed += speed_update_scale;
        }

        speed = Mathf.Clamp(speed, min_speed, 0.3f);
        var cur_speed = 0f;
        var direction = Vector3.up;

        if (transform.position.y >= end_of_world_y)
            transform.Translate(new Vector3(0, end_of_world_y - transform.position.y));

        if (transform.position.y < 0)
            transform.Translate(new Vector3(0, -transform.position.y));

        if(transform.position.x < 0)
            transform.Translate(new Vector3(-transform.position.x, 0));

        if(transform.position.x >= end_of_world_x_2)
            transform.Translate(new Vector3(end_of_world_x_2 - transform.position.x, 0));

        if(transform.position.y == end_of_world_y && transform.position.x >= end_of_world_x_1)
            transform.Translate(new Vector3(end_of_world_x_1 - transform.position.x, 0));

        if (IsBetween(transform.position.y, 0, end_of_world_y) && transform.position.x == 0)
        {
            direction = Vector3.up;
            cur_speed = speed;
        }
        if (transform.position.y == end_of_world_y && transform.position.x <= end_of_world_x_1)
        {
            direction = Vector3.right;
            cur_speed = speed;
        }

        if (IsBetween(transform.position.y, 10, end_of_world_y + 1f) && transform.position.x == end_of_world_x_1)
        {
            direction = Vector3.down;
            cur_speed = speed;
        }

        if (transform.position.y <= 10 && IsBetween(transform.position.x, end_of_world_x_1, end_of_world_x_2))
        {
            direction = Vector3.right;
            cur_speed = speed;
        }

        if (IsBetween(transform.position.y, 0, 10) && transform.position.x == end_of_world_x_2)
        {
            direction = Vector3.down;
            cur_speed = speed;
        }

        if (IsBetween(transform.position.x, end_of_world_x_1, end_of_world_x_2 + 1f) && transform.position.y == 0)
        {
            direction = Vector3.left;
            cur_speed = speed;
        }

        transform.Translate(cur_speed * direction, Space.World);
    }
}
