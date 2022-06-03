using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float scrollSpeed = 50f;
    public Vector2 panLimit;

    [SerializeField]
    private Camera cam;

    public float width;
    public float height;

    void Update()
    {
        move(); scroll();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

    public void move()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);

        transform.position = pos;
    }

    private void scroll()
    {
        var input = Input.GetAxisRaw("Mouse ScrollWheel");

        if (cam.orthographicSize <= 40)
        {
            if (input > 0)
            {
                input = 0;
            }
        }
        if (cam.orthographicSize >= 312.5)
        {
            if (input < 0)
            {
                input = 0;
            }
        }
        if (cam.orthographicSize > 312.5)
        {
            cam.orthographicSize = 312.5f;
        }
        if (cam.orthographicSize < 40)
        {
            cam.orthographicSize = 40;
        }
        cam.orthographicSize -= input * scrollSpeed;
        float ortho = cam.orthographicSize;

        panSpeed = 1.65138f * ortho + 33.945f;
        panLimit.x = (1425f - width) / 2f;
        panLimit.y = 312.5f - ortho;
    }

}
