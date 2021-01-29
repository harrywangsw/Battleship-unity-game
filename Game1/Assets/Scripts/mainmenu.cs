using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class mainmenu : NetworkBehaviour
{
    public GameObject b, c, s, draged;
    public GameObject[,] text = new GameObject[42, 36];
    public int interval;
    int i, j, shipnum;
    public GameObject[] playership;
    Transform[] shipui;
    bool placeable;
    Camera cam;
    void Start()
    {
        b = GameObject.Find("networkmanager");
        c = GameObject.Find("Camera");
        cam = c.GetComponent<Camera>();
        GameObject.Find("maincanvas").GetComponent<Canvas>().enabled = false;
        interval = 16;
        defineobjects(0);
        playership = GameObject.FindGameObjectsWithTag("ship");

    }

    void Update()
    {
        if (s)
        {
            Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            s.transform.position = new Vector3(pos.x, pos.y, 0);
            for (i = 0; i <= 41; i++)
            {

                for (j = 0; j <= 25; j++)
                {
                    if ((s.transform.position - text[i,j].transform.position).magnitude <12.0f)
                    {
                        s.transform.position = text[i, j].transform.position;
                        if (Input.GetMouseButtonDown(1))
                        {
                            playership[shipnum].transform.position = text[i,j].transform.position;
                            Destroy(s);
                            Destroy(draged);
                        }
                    }
                }
            }
        }
    }
    public void startmultiplayer()
    {
        b.GetComponent<NetworkManagerHUD>().showGUI = true;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        c.transform.position = new Vector3(0, 0, -54.45f);
    }
    public void defineobjects(int drawplace)
    {

        for (i = 0; i <= 41; i++)
        {

            for (j = 0; j <= 25; j++)
            {
                text[i, j] = (GameObject)(Instantiate(GameObject.Find("probabilitytext"), new Vector3(0,0,0), Quaternion.identity));
                text[i, j].transform.SetParent(GameObject.Find("gridcanvas").transform);
                text[i, j].transform.position = new Vector3(drawplace + i * interval, j * interval, 0.0f);

            }
        }
    }
    public void playerchoose()
    {
        c.AddComponent<localcam>();
        GameObject.Find("shipcanvas").GetComponent<CanvasScaler>().scaleFactor = 1;

    }
    public void drag(GameObject dragship)
    {
        dragship.GetComponent<Image>().color = new Color(255.0f, 255,0f, 100.0f);
        draged = dragship;
        for(i=0; i<=4; i++)
        {
            if(playership[i].GetComponent<SpriteRenderer>().sprite == dragship.GetComponent<Image>().sprite)
            {
                shipnum = i;   
            }
        }
        s = (GameObject)(Instantiate(playership[shipnum], cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), Quaternion.identity));
        s.transform.localScale = new Vector3(8, 8, 0);
    }
}
