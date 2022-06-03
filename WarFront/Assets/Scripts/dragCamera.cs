using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragCamera : MonoBehaviour
{
    public static dragCamera instance;

    [Header("Settings")]
    public float moveSpeed;
    public float scrollSpeed = 5f;

    [SerializeField]
    private Camera cam;
    private Vector3 dragOrigin;
    public Vector2 dragLimit;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        drag();
        scroll();
        Move();
    }

    private void drag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 pos = transform.position;
            var mousePos = Input.mousePosition;
            Vector3 dif = dragOrigin - cam.ScreenToWorldPoint(mousePos);

            pos += dif;

            //pos.x = Mathf.Clamp(pos.x, -dragLimit.x, dragLimit.x);
            //pos.y = Mathf.Clamp(pos.y, -dragLimit.y, dragLimit.y);

            transform.position = pos;
        }

    }

    private void scroll()
    {
        var input = Input.GetAxisRaw("Mouse ScrollWheel");

        //if (cam.orthographicSize == 1)
        //{
        //    if (input > 0)
        //    {
        //        input = 0;
        //    }
        //}
        //if (cam.orthographicSize == 6)
        //{
        //    if (input < 0)
        //    {
        //        input = 0;
        //    }
        //}

        cam.orthographicSize -= input * scrollSpeed;

        float ortho = cam.orthographicSize;
        ortho = (ortho / 2);

        if (ortho < 1)
            ortho = 1;

        ortho = 4 / ortho;

        //dragLimit = new Vector2(ortho, ortho);

    }

    public void Move()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0);
    }

    public void GoTo(Vector3 pos)
    {
        //Debug.Log("Camera GoTo Called");
        Vector3 camPos = transform.position;
        Vector3 npos = new Vector3(pos.x, pos.y, camPos.z);
        //Vector2 npos = new Vector2(pos.x, pos.y);

        transform.position = npos;
        cam.orthographicSize = 1.5f;
    }
}