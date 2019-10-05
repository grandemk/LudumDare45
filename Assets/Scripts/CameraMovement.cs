using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float speed = 0.01f;
    void Update()
    {
        transform.Translate(speed * new Vector3(0, 1), Space.World);
    }
}
