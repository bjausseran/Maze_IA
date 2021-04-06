using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Camera cam;
    float moveSpeed = 0.01f;
    float zoomSpeed = 0.1f;
    Vector3 onClickPositon;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        var xMouse = transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var yMouse = transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        if (!IsMouseOverUI())
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;
        }
        if (Input.GetMouseButtonDown(2))
        {
            onClickPositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //if (Mathf.Abs(xMouse) > 6.4f || Mathf.Abs(yMouse) > 4f)
        if (Input.GetMouseButton(2))
        {
            var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - onClickPositon;
            transform.position = Vector3.Slerp(transform.position, transform.position + dir, moveSpeed);
        }
        
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
