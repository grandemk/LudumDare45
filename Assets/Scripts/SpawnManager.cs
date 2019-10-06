using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool is_running;
    private IEnumerator spawn_coroutine;

    [SerializeField]
    private GameObject basic_food_prefab;
    [SerializeField]
    private GameObject angry_food_prefab;
    [SerializeField]
    private GameObject mad_food_prefab;

    [SerializeField]
    private GameObject spawn_container;

    [SerializeField]
    private int max_simultaneous_spawn = 20;

    [SerializeField]
    private int min_simultaneous_spawn = 2;

    [SerializeField]
    private float spawn_interval_delay = 2.5f;

    public Player player;
    float hunger_meter_trend = 0f;
    float time_hunger_check;

    [SerializeField]
    private Camera cam;

    void Start()
    {
        time_hunger_check = Time.time;
        is_running = true;
        spawn_coroutine = SpawnRoutine();
        StartCoroutine(spawn_coroutine);
    }

    void Update()
    {
        if (Time.time >= time_hunger_check)
        {
            time_hunger_check += 1f;
            var cur_hunger_meter = 0;
            if (player == null)
                cur_hunger_meter = player.hunger_meter;

            hunger_meter_trend = cur_hunger_meter - hunger_meter_trend;
        }
    }

    private int ChooseSpawnNumber(int hunger_meter, float dist)
    {
        int num_spawn = 1;
        if (dist < 0.5f)
            num_spawn = Mathf.Clamp(num_spawn, 2, hunger_meter / 1000);

        return num_spawn;
    }

    private GameObject ChooseSpawnType(int hunger_meter, float dist)
    {
        if (hunger_meter > 3000)
        {
            if (dist < 0.3f)
                return angry_food_prefab;
            else if(dist < 0.8f)
                return basic_food_prefab;
            else
                return mad_food_prefab;
        }

        if (hunger_meter > 1000)
        {
            if (dist < 0.3f)
                return angry_food_prefab;
            else
                return basic_food_prefab;
        }

        return basic_food_prefab;
    }

    private IEnumerator SpawnRoutine()
    {
        while(is_running)
        {
            var num_simultaneous_spawn = spawn_container.transform.childCount;
            if (num_simultaneous_spawn < max_simultaneous_spawn)
            {
                var hunger_meter = 0;
                if (player != null)
                    hunger_meter = player.hunger_meter;

                int spawn_num = ChooseSpawnNumber(hunger_meter, Random.Range(0f, 1f));
                
                for (int i = 0; i < spawn_num; ++i)
                {
                    var spawned_prefab = ChooseSpawnType(hunger_meter, Random.Range(0f, 1f));
                    GameObject obj = Instantiate(spawned_prefab, spawn_container.transform);
                    Vector3 world_pos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2 + Random.Range(-Screen.width / 2 + 1, Screen.width / 2 - 1), Screen.height / 2 + Random.Range(-Screen.height / 2 + 1, Screen.height / 2 - 1), 0));
                    world_pos.z = 0f;
                    obj.transform.position = world_pos;
                }
            }

            if (hunger_meter_trend < 20f)
                spawn_interval_delay = 1f;

            if (num_simultaneous_spawn < min_simultaneous_spawn)
                spawn_interval_delay = 0.5f;

            yield return new WaitForSeconds(spawn_interval_delay);
        }
    }
}
