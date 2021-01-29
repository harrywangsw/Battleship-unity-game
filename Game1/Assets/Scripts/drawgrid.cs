using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
public class drawgrid : NetworkBehaviour
{
    Vector3 gridx, gridy;
    int interval;
    int i, j, k;
    public GameObject[] myship;
    public GameObject[] enemyship, empty = new GameObject[5];
    GameObject lastship, hitsrecorder;
    public Material outline, original;
    public Sprite sea, end;
    bool[,] revealed = new bool[42, 36];
    float[,,] probability = new float[42, 36, 12];
    public GameObject[,] dot = new GameObject[42, 36];
    public bool scoutable, attackable, flip, movable = false;
    public Sprite scouted, unscouted;
    public int[] hitted, hits;
    public  int[] health = new int[] { 1, 2, 3, 4, 5 };

    void Start()
    {
        for (i = 1; i <= 5; i++)
        {
            health[i] = i;
        }
        hitted = new int[5];
        hits = new int[5];
        interval = GameObject.Find("Battleships-1200x675").GetComponent<mainmenu> ().interval;
        for (i = 0; i <= 41; i++)
        {
            for (j = 0; j <= 25; j++)
            {
                dot[i, j] = GameObject.Find("Battleships-1200x675").GetComponent<mainmenu>().text[i, j];
            }
        }
        for(i=0; i<=4; i++)
        {
            hits[i] = 0;
            hitted[i] = 0;
        }
        myship = GameObject.FindGameObjectsWithTag("ship");
        enemyship = GameObject.FindGameObjectsWithTag("empty");
        empty = GameObject.FindGameObjectsWithTag("empty");
        hitsrecorder = GameObject.Find("hit recorder");
        
        if (gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            gameObject.name = "localplayer";

        }
        else
        {
            gameObject.name = "otherplayer";
        }
        if (gameObject.transform.position == GameObject.Find("Spawn2").transform.position)
        {

            gameObject.tag = "2";
            if (isLocalPlayer)
            {
                
                for (i = 0; i <= 4; i++)
                {
                    empty[i].transform.position = myship[i].transform.position;
                    Debug.Log("starting empty location: " + empty[i].transform.position);
                }
            }
        }
        else
        {
            gameObject.tag = "1";
        }

    }

    
    void Update()
    {
        quantumfield(enemyship);
        /*if (gameObject.tag == "1" && isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Cmdchangeposition();
            }
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                CurrentClickedGameObject(raycastHit.transform.gameObject);
                Debug.Log(raycastHit.transform.gameObject.name);
            }           
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            displaytactical();
        }
        if (movable)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lastship.transform.position = new Vector3(lastship.transform.position.x - interval, lastship.transform.position.y, lastship.transform.position.z);
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                lastship.transform.position = new Vector3(lastship.transform.position.x + interval, lastship.transform.position.y, lastship.transform.position.z);
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                lastship.transform.position = new Vector3(lastship.transform.position.x, lastship.transform.position.y+interval, lastship.transform.position.z);
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                lastship.transform.position = new Vector3(lastship.transform.position.x, lastship.transform.position.y-interval, lastship.transform.position.z);
                movable = false;
            }
        }
        recieveenemy();
        Debug.Log("enemyship: "+enemyship[0].transform.position);
        Debug.Log("empty: "+empty[2].transform.position);
        
    }

    void CurrentClickedGameObject(GameObject dot)
    {
        if (dot.tag == "dot1" || dot.tag == "dot2")
        {
            float p = 0.0f;

            float x = (dot.transform.position.x) / interval;
            float y = (dot.transform.position.y) / interval;

            string coordinate = "(" + ((dot.transform.position.x) / interval).ToString() + "," + ((dot.transform.position.y) / interval).ToString() + ")";
            GameObject.FindWithTag("text").GetComponent<Text>().text = coordinate;

            for (k = 0; k < enemyship.Length; k++)
            {

                p += probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k];
                if (p >= 500)
                {
                    p = 500;
                }
            }

            if (scoutable)
            {
                Debug.Log("scouting");
                dot.GetComponent<Text>().text = (p / 5.0f).ToString() + "%";
                revealed[Mathf.RoundToInt(x), Mathf.RoundToInt(y)] = true;
                GameObject.Find("scout").transform.localScale = new Vector3(0, 0, 0);
                gameObject.GetComponent<changesprite>().ChangeTileTexture(new Vector3Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0), scouted);
            }

            if (attackable == true)
            {
                int[] value = new int[1001];
                for (j = 0; j <= 10 * Mathf.Round(p / 5.0f); j++) {
                    value[j] = 1;
                }
                
                
                for(i = 0; i <= 10 * (100-Mathf.Round(p / 5.0f)); i++)
                {
                    value[i] = 0;

                }
                if(value[Random.Range(0, value.Length)] == 0)
                {
                    Debug.Log("missed");
                    GameObject.Find("hit").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("hit").GetComponent<Text>().text = "missed";   
                }
                else
                {
                    Debug.Log("hit!");
                    GameObject.Find("hit").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("hit").GetComponent<Text>().text = "hit";
                    calhit(x,y);
                    for(i=0; i<=4; i++)
                    {
                        if(hits[i] == health[i + 1])
                        {
                            GameObject.Find("hit").GetComponent<Text>().text = "hit!"+enemyship[i].name+" destroyed";
                        }
                    }

                }
                GameObject.Find("attack").transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else if (dot.tag == "ship")
        {
            Debug.Log("clickedship");
            if (lastship)
            {
                lastship.GetComponent<SpriteRenderer>().material = original;
            }
            dot.GetComponent<SpriteRenderer>().material = outline;
            lastship = dot;
            drawquantumfield(dot);
            move(dot);
        }
    }

    void quantumfield(GameObject[] ship)
    {
        
        
        for(i=0; i<= 41; i++)
        {
            for(j=0; j<=25; j++)
            {
                for (k = 0; k < ship.Length; k++)
                {
                    Vector3 a = ship[k].transform.position - dot[i, j].GetComponent<Transform>().position;
                    float p = a.magnitude;
                    probability[i,j,k] = Mathf.Pow(0.98f, p)*10000/8800*100;
                }
            }
        }
    }

    public void drawquantumfield(GameObject ships)
    {
        quantumfield(myship);
        for (i = 0; i <= 41; i++)
        {
            for (j = 0; j <= 25; j++)
            {
                for (k = 0; k < enemyship.Length; k++)
                {
                    if (ships == myship[k])
                    {

                        dot[i, j].GetComponent<Text>().text = probability[i, j, k].ToString() + "%";

                    }
                }
            }
        }
        quantumfield(enemyship);
    }

    public void displaytactical()
    {
        
        GameObject.Find("return").transform.localScale = new Vector3(1, 1, 1);
        for(i=0; i<enemyship.Length; i++)
        {
            myship[i].transform.localScale = new Vector3(0,0,0);
        }

        for (i = 0; i <= 41; i++)
        {
            for (j = 0; j <= 25; j++)
            {
               

                if (!revealed[i, j])
                {
                    dot[i, j].GetComponent<Text>().text = "  ";
                    dot[i, j].GetComponent<Text>().color = Color.red;

                }
            }

        }
    }

    public void scout()
    {
        Start();
        displaytactical();
        scoutable = true;
        GameObject.Find("scout").transform.localScale = new Vector3(0, 0, 0);
    }


    public void goback()
    {
        scoutable = false;
        attackable = false;
        flip = false;
        for (i = 0; i < enemyship.Length; i++)
        {
            myship[i].transform.localScale = new Vector3(18, 18, 18);
        }
        drawquantumfield(lastship);

        GameObject.Find("return").transform.localScale = new Vector3(0, 0, 0);
        if (scoutable)
        {
            GameObject.Find("scout").transform.localScale = new Vector3(1, 1, 1);
        }
        for (i=0; i <= 41; i++)
        {
            for (j = 0; j <= 25; j++) {
                dot[i, j].GetComponent<Text>().color = new Color(0, 255, 0, 225);
                gameObject.GetComponent<changesprite>().ChangeTileTexture(new Vector3Int(i, j, 0), unscouted);
            }
        }
    }

    public void attack()
    {
        Start();
        displaytactical();

        attackable = true;
    }

    public void move(GameObject ship)
    {
        //GameObject b = (GameObject)(Instantiate(ship, new Vector3(ship.transform.position.x + interval, ship.transform.position.y, ship.transform.position.z), Quaternion.identity));
        //b.GetComponent<SpriteRenderer>().color = new Color(b.GetComponent<SpriteRenderer>().color.r, b.GetComponent<SpriteRenderer>().color.g, b.GetComponent<SpriteRenderer>().color.b, 88);
        //b.tag = "clone";
        movable = true;
    }

    public void calhit(float x, float y)
    {
        int returnvar = 0;
        for(k=0; k<=4; k++)
        {
            float min = 0.0f;
            if(probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k] >= min)
            {
                min = probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k];
                returnvar = k;
            }
        }
        for(i=0; i<=4; i++)
        {
            if(returnvar == i)
            {
                hits[i] += 1;
            }
        }
    }

    public void recieveenemy()
    {
        for (i = 0; i <= 4; i++)
        {
            enemyship[i].transform.position = empty[i].transform.position;
        }
        hitted[0] = Mathf.RoundToInt(hitsrecorder.transform.position.x);
        hitted[1] = Mathf.RoundToInt(hitsrecorder.transform.position.y);
        hitted[2] = Mathf.RoundToInt(hitsrecorder.transform.position.z);
        hitted[3] = Mathf.RoundToInt(hitsrecorder.transform.rotation.x);
        hitted[4] = Mathf.RoundToInt(hitsrecorder.transform.rotation.y);

        if (Mathf.Round(GameObject.Find("record active player").transform.position.x) == 2 && gameObject.tag == "2" && isLocalPlayer)
        {
            GameObject.Find("maincanvas").GetComponent<CanvasScaler>().scaleFactor = 1;
        }
        if (Mathf.Round(GameObject.Find("record active player").transform.position.x) == 1 && gameObject.tag == "1" && isLocalPlayer)
        {
            GameObject.Find("maincanvas").GetComponent<CanvasScaler>().scaleFactor = 1;
        }
        if (Mathf.Round(GameObject.Find("record active player").transform.position.x).ToString() != gameObject.tag && isLocalPlayer)
        {
            GameObject.Find("maincanvas").GetComponent<CanvasScaler>().scaleFactor = 0.01f;
        }
    }

    [Command]
    public void Cmdsetenemy(Vector3[] playershippos, int[] hits)
    {
        for(i=0; i<=4; i++)
        {
            Debug.Log(empty[i].transform.position);
            empty[i].transform.position = playershippos[i];
        }

        hitsrecorder.transform.position = new Vector3(hits[0], hits[1], hits[2]);
        hitsrecorder.transform.localScale = new Vector3(hits[3], hits[4], 0);
        Debug.Log("enemysetted");

        if (Mathf.Round(GameObject.Find("record active player").transform.position.x) == 2)
        {
            GameObject.Find("record active player").transform.position = new Vector3(1, 0, 0);
        }
        if(Mathf.Round(GameObject.Find("record active player").transform.position.x) == 1)
        {
            GameObject.Find("record active player").transform.position = new Vector3(2, 0, 0);
        }
    }

  

   
}
