using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private int main_menu_scene_index = 0;
    private int game_scene_index = 1;
    private float time_before_main_menu = 2f;
    private float go_back_to_main_menu_time;
    private bool started_end_transition = false;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        SceneLoader loader = GetComponent<SceneLoader>();
        if (Input.GetKeyDown(KeyCode.Escape))
            loader.LoadByIndex(main_menu_scene_index);
        if (Input.GetKeyDown(KeyCode.R))
            loader.LoadByIndex(game_scene_index);

        if (player == null)
        {
            if(!started_end_transition)
                go_back_to_main_menu_time = Time.time + time_before_main_menu;
            started_end_transition = true;
        }
        else
        {
            go_back_to_main_menu_time = Time.time + 1f;
        }

        if(Time.time > go_back_to_main_menu_time)
            loader.LoadByIndex(main_menu_scene_index);
    }
}
