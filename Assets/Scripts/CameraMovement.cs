using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.01f;

    [SerializeField]
    private float end_of_world_y = 30f;

    void Update()
    {
        if(transform.position.y < end_of_world_y)
            transform.Translate(speed * new Vector3(0, 1), Space.World);
    }
}
