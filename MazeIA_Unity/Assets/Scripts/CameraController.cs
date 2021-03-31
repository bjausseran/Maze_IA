using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed = 0.01f;

    void Update()
    {
        var xMouse = transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var yMouse = transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        if (Mathf.Abs(xMouse) > 6.4f || Mathf.Abs(yMouse) > 4f)
        {
            Debug.Log("CameraController, Update : x = " + xMouse + ", y = " + yMouse);
            var aim = Vector3.right * Camera.main.ScreenToWorldPoint(Input.mousePosition).x +
                        Vector3.up * Camera.main.ScreenToWorldPoint(Input.mousePosition).y +
                            Vector3.forward * -10f;
            transform.position = Vector3.Slerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * -10f, speed);
        }
    }
}
