using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localcam : MonoBehaviour
{
    Vector3 pos, scale;
    Vector2 velocity;
    int i;
    void Start()
    {
        pos = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * 13.0f;
        velocity.y = Input.GetAxis("Vertical") * 13.0f;


        pos.x += velocity.x;
        pos.y += velocity.y;
        if (pos.x >= 680 || pos.x <= 0)
        {
            pos.x -= velocity.x;
        }
        if (pos.y >= 380 || pos.y <= 0)
        {
            pos.y -= velocity.y;
        }


        gameObject.transform.position = pos;
        pos.z += 10;
        pos.z -= 10;
        gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize - Input.mouseScrollDelta.y * 2.0f;
        if (gameObject.GetComponent<Camera>().orthographicSize <= 0 || gameObject.GetComponent<Camera>().orthographicSize >= 90)
        {
            gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize + Input.mouseScrollDelta.y * 2.0f;
        }
    }
}
