using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadFood : Food
{
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private CameraMovement cam_movement;

    Vector3 init_position;
    private float leash_size = 2f;
    private float direction_change_time = 2f;
    private Vector3 current_direction;

    private float cam_speed_up = 0f;
    private float speed_up_time = 0f;

    void Start()
    {
        init_position = transform.position;
        satiation_value = 200;
        speed_up_time = Time.time;
        var cam = Camera.main;
        cam_movement = cam.GetComponent<CameraMovement>();
    }

    void Drain()
    {
        var obj = GameObject.Find("Player");
        if (obj == null)
            return;
        var player = obj.GetComponent<Player>();
        if (player == null)
            return;
        player.hunger_meter -= 2;
    }

    void SpeedUp()
    {
        var speed_up = 0.01f;
        if (cam_speed_up < 0.1f)
        {
            cam_speed_up += speed_up;
            cam_movement.speed += speed_up;
        }
    }

    private void OnDestroy()
    {
        cam_movement.speed -= cam_speed_up;
    }

    public GameObject FindClosestFoodToConvert()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("BasicFood");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void Update()
    {
        Drain();
        if (Time.time > speed_up_time)
        {
            speed_up_time = Time.time + 1f;
            SpeedUp();
        }

        var closest_food = FindClosestFoodToConvert();
        if (closest_food == null)
            Debug.Log("target position null");
        if (closest_food != null)
        {
            direction_change_time += Time.time;
            current_direction = Vector3.Normalize(closest_food.transform.position - transform.position);
            Debug.Log("direction: " + current_direction);
            Debug.Log("target position" + closest_food.transform.position);
            Debug.Log("our position" + transform.position);
            transform.Translate(Time.deltaTime * speed * current_direction);
            init_position = transform.position;
            transform.Translate(Time.deltaTime * speed * current_direction);
        }
        else
        {
            if (Time.time > direction_change_time)
            {
                direction_change_time += Time.time;
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                current_direction = Vector3.Normalize(new Vector3(x, y, 0));
            }

            transform.Translate(Time.deltaTime * speed * current_direction);
            float current_distance = Vector3.Distance(transform.position, init_position);
            if (current_distance > leash_size)
            {
                transform.Translate(current_direction * (leash_size - current_distance));
                direction_change_time = Time.time;
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Feed(this);
                Destroy(this.gameObject);
            }
            else
                Debug.LogWarning("Player doesn't have any script enabled !");
        }

        if (other.CompareTag("BasicFood"))
        {
            Destroy(other.gameObject);
        }
    }

}
