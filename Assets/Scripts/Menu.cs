using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private int main_menu_scene_index = 0;
    void Update()
    {
        SceneLoader loader = GetComponent<SceneLoader>();
        if (Input.GetKeyDown(KeyCode.Escape))
            loader.LoadByIndex(main_menu_scene_index);
    }
}
