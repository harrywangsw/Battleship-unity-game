using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class movecam : NetworkBehaviour
{
    Vector3 pos, scale;
    Vector2 velocity;
    void Start()
    {
        Destroy(GameObject.Find("Camera"));
        if (isLocalPlayer)
        {
            pos = gameObject.transform.position;
            Camera.main.transform.position = this.transform.position;
            Camera.main.transform.LookAt(this.transform.position);
            Camera.main.transform.parent = this.transform;
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            velocity.x = Input.GetAxis("Horizontal") * 13.0f;
            velocity.y = Input.GetAxis("Vertical") * 13.0f;


            pos.x += velocity.x;
            pos.y += velocity.y;
            if (pos.x >= 680 || pos.x <= 0)
            {
                pos.x -= velocity.x;
            }
            if (pos.y >= 290 || pos.y <= 0)
            {
                pos.y -= velocity.y;
            }

            gameObject.transform.position = pos;
            pos.z += 10;
            pos.z -= 10;
            Camera.main.GetComponent<Camera>().orthographicSize = Camera.main.GetComponent<Camera>().orthographicSize - Input.mouseScrollDelta.y * 2.0f;
            if (Camera.main.GetComponent<Camera>().orthographicSize <= 0|| Camera.main.GetComponent<Camera>().orthographicSize >= 90)
            {
                Camera.main.GetComponent<Camera>().orthographicSize = Camera.main.GetComponent<Camera>().orthographicSize + Input.mouseScrollDelta.y * 2.0f;
            }
        }
    }
}
