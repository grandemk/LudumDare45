using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFood : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.5f;
    public int satiation_value = 250;

    Vector3 init_position;
    float leash_size = 2f;
    float direction_change_time = 2f;
    Vector3 current_direction;

    private void Start()
    {
        init_position = transform.position;
    }

    void Update()
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

    private void OnTriggerEnter(Collider other)
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
    }
}
