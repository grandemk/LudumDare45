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
    private int max_simultaneous_spawn = 10;

    [SerializeField]
    private float spawn_interval_delay = 2.5f;

    public Player player;

    [SerializeField]
    private Camera cam;

    void Start()
    {
        is_running = true;
        spawn_coroutine = SpawnRoutine();
        StartCoroutine(spawn_coroutine);
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
                var dist = Random.Range(0f, 1f);
                var spawned_prefab = ChooseSpawnType(player.hunger_meter, dist);
                GameObject obj = Instantiate(spawned_prefab, spawn_container.transform);

                Vector3 center = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                center.z = 0;
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                float distance = Random.Range(0f, 8f);
                var direction = Vector3.Normalize(new Vector3(x, y, 0));
                var final_position = center + direction * distance;
                obj.transform.position = final_position;
            }
            yield return new WaitForSeconds(spawn_interval_delay);
        }
    }
}
