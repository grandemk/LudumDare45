using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var loader = GetComponent<SceneLoader>();
            loader.LoadByIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            var app_manager = GetComponent<ApplicationManager>();
            app_manager.Quit();
        }
    }
}
