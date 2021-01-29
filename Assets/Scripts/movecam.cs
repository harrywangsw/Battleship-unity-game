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
            //if (gameObject.name == "focus1" && (pos.x >= 328 || pos.x <= -20))
            //{
            //pos.x -= velocity.x;
            //}

            //if (gameObject.name == "focus1" && (pos.y <= -20 || pos.y >= 288))
            //{
            //pos.y -= velocity.y;
            //}

            gameObject.transform.position = pos;
            pos.z += 10;
            GameObject.Find("background").transform.position = pos;
            pos.z -= 10;
            Camera.main.GetComponent<Camera>().orthographicSize = Camera.main.GetComponent<Camera>().orthographicSize - Input.mouseScrollDelta.y * 2.0f;
            if (Camera.main.GetComponent<Camera>().orthographicSize <= 0)
            {
                Camera.main.GetComponent<Camera>().orthographicSize = Camera.main.GetComponent<Camera>().orthographicSize + Input.mouseScrollDelta.y * 2.0f;
            }
            scale.x = Camera.main.GetComponent<Camera>().orthographicSize * 189.8f;
            scale.y = Camera.main.GetComponent<Camera>().orthographicSize * 204.5f;
            GameObject.Find("background").transform.localScale = scale;
        }
    }
}
