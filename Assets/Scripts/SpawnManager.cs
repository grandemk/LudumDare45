using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool is_running;
    private IEnumerator spawn_coroutine;

    [SerializeField]
    private GameObject spawned_prefab;

    [SerializeField]
    private GameObject spawn_container;

    [SerializeField]
    private int max_simultaneous_spawn = 10;

    void Start()
    {
        is_running = true;
        spawn_coroutine = SpawnRoutine();
        StartCoroutine(spawn_coroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while(is_running)
        {
            var num_simultaneous_spawn = spawn_container.transform.childCount;
            if (num_simultaneous_spawn < max_simultaneous_spawn)
            {
                GameObject obj = Instantiate(spawned_prefab);
                obj.transform.SetParent(spawn_container.transform);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
