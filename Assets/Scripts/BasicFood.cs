using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFood : Food 
{
    [SerializeField]
    private float speed = 2.5f;

    public Sprite idle1_sprite;
    public Sprite idle2_sprite;

    Vector3 init_position;
    float leash_size = 2f;
    float direction_change_time = 2f;
    Vector3 current_direction;
    float anim_idle_change_time;
    bool anim_toggle = false;

    void Start()
    {
        init_position = transform.position;
        satiation_value = 250;
        anim_idle_change_time = Time.time;
    }

    void UpdateSprite(Sprite new_sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = new_sprite; 
    }

    void Update()
    {
        if (Time.time > anim_idle_change_time)
        {
            anim_idle_change_time = Time.time + 0.5f;
            anim_toggle = !anim_toggle;
            if (anim_toggle)
                UpdateSprite(idle2_sprite);
            else
                UpdateSprite(idle1_sprite);
        }

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
