using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerMeter : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        var slider = GetComponent<Slider>();
        if(player != null)
            slider.value = (float)player.hunger_meter / (float)player.max_hunger_meter;
    }
}
