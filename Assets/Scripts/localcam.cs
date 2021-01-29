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
        gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize - Input.mouseScrollDelta.y * 2.0f;
        if (gameObject.GetComponent<Camera>().orthographicSize <= 0)
        {
            gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize + Input.mouseScrollDelta.y * 2.0f;
        }
        scale.x = gameObject.GetComponent<Camera>().orthographicSize * 189.8f;
        scale.y = gameObject.GetComponent<Camera>().orthographicSize * 204.5f;
        GameObject.Find("background").transform.localScale = scale;
        for(i=0; i<=4; i++)
        {
            GameObject.Find("Battleships-1200x675").GetComponent<mainmenu>().playership[i].transform.localScale = Vector3.one*gameObject.GetComponent<Camera>().orthographicSize/10;
        }
    }
}
