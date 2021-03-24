using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed = 0.01f;

    void Update()
    {
        if (Mathf.Abs(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) > 6.4f 
            || Mathf.Abs(transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y) > 4f)
        {
            transform.position = Vector3.Slerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * -10f, speed);
        }
    }
}
