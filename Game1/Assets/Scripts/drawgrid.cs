using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.Tilemaps;
public class drawgrid : NetworkBehaviour
{
    Vector3 gridx, gridy;
    Vector3[] temp = new Vector3[5];
    int interval;
    int i, j, k;
    public GameObject[] enemyship, empty, myship = new GameObject[5];
    GameObject lastship, hitsrecorder;
    public Material outline, original;
    public Sprite sea, end;
    bool[,] revealed = new bool[42, 36];
    float[,,] probability = new float[42, 36, 12];
    public GameObject[,] dot = new GameObject[42, 36];
    public bool scoutable, attackable, flip, movable;
    public Sprite scouted, unscouted, radar;
    public int[] hitted, hits;
    public  int[] health = new int[] {1,2,3,4,5};
    public int shipdestroyed, scoutamount, ROCscout, scoutarea, damage = 0;
    public Tilemap tilemap;

    void Start()
    {
        /*for (i = 0; i <= 4; i++)
        {
            health[i] = i+1;
        }*/
        hitted = new int[5];
        hits = new int[5];
        interval = GameObject.Find("Battleships-1200x675").GetComponent<mainmenu> ().interval;
        damage = 1;
        countrymanager();
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
        definemyship();
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
                }
            }
        }
        else
        {
            gameObject.tag = "1";
        }

    }

    // Update is called once per frame
    void Update()
    {
        quantumfield(empty);
        if (Input.GetMouseButtonDown(0)&&isLocalPlayer)
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
        if (movable&&isLocalPlayer&&lastship)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lastship.transform.rotation = Quaternion.Euler(Vector3.forward*90);
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                lastship.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (lastship.transform.rotation.z == 90 || lastship.transform.rotation.z == -90)
                {
                    lastship.transform.position += new Vector3(interval, 0, 0);
                }
                else
                {
                    lastship.transform.position = new Vector3(lastship.transform.position.x, lastship.transform.position.y + interval, lastship.transform.position.z);
                }
                movable = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (lastship.transform.rotation.z == 90 || lastship.transform.rotation.z == -90)
                {
                    lastship.transform.position -= new Vector3(interval, 0, 0);
                }
                else
                {
                    lastship.transform.position = new Vector3(lastship.transform.position.x, lastship.transform.position.y - interval, lastship.transform.position.z);
                }
                movable = false;
            }
        }
        recieveenemy();
        if(gameObject.tag == "2"&& isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameObject.Find("record active player").transform.position += new Vector3(8, 0, 0);
            }
        }
        Debug.Log(GameObject.Find("hit recorder").transform.position);
        Debug.Log(GameObject.Find("hit recorder").transform.rotation);

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
                Debug.Log("probability" + probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k]);
            }

            if (scoutable)
            {
                Debug.Log("scouting");
                dot.GetComponent<Text>().text = (p / 5.0f).ToString() + "%";
                GameObject.Find("scout").transform.localScale = new Vector3(0, 0, 0);
                tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
                for (i = -scoutarea; i <= scoutarea; i++)
                {
                    for (j = -scoutarea; j <= scoutarea; j++)
                    {
                        revealed[Mathf.RoundToInt(x) + i, Mathf.RoundToInt(y)+j] = true;
                        gameObject.GetComponent<changesprite>().ChangeTileTexture(tilemap, new Vector3Int(Mathf.RoundToInt(x)+i, Mathf.RoundToInt(y)+j, 0), scouted);
                    }
                }
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
                    GameObject.Find("hit").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("hit").GetComponent<Text>().text = "missed";
                    
                }
                else
                {
                    GameObject.Find("hit").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("hit").GetComponent<Text>().text = "hit";
                    Debug.Log("hit");
                    calhit(x,y);
                    

                }
                Debug.Log(p);
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
                    probability[i, j, k] = Mathf.Pow(0.998f, p)*100;
                    //probability[i,j,k] = 100-p*0.1f;
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
        for (i=0; i<enemyship.Length; i++)
        {
            myship[i].transform.localScale = new Vector3(0,0,0);
        }


        for (i = 0; i <= 41; i++)
        {
            for (j = 0; j <= 25; j++)
            {
                gameObject.GetComponent<changesprite>().ChangeTileTexture(GameObject.Find("Tilemap").GetComponent<Tilemap>(), new Vector3Int(i, j, 0), radar);
                GameObject.Find("tactical grid").transform.localScale = new Vector3(1, 1, 1);
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
        displaytactical();
        scoutamount += 1;
        if (scoutamount <= ROCscout+1)
        {
            scoutable = true;
        }
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
                GameObject.Find("tacticalTilemap").transform.position = new Vector3(1, 1, 1);
                gameObject.GetComponent<changesprite>().ChangeTileTexture(GameObject.Find("Tilemap").GetComponent<Tilemap>(), new Vector3Int(i, j, 0), unscouted);
            }
        }
    }

    public void attack()
    {
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
        float min = 0.0f;
        for (k=0; k<=4; k++)
        {
            if(probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k] >= min)
            {
                min = probability[Mathf.RoundToInt(x), Mathf.RoundToInt(y), k];
                returnvar = k;
            }
        }
        Debug.Log(myship[returnvar].name);
        for(i=0; i<=4; i++)
        {
            if(returnvar == i)
            {
                hits[i] += damage;
                if (hits[i] >= health[i])
                {
                    GameObject.Find("hit").GetComponent<Text>().text = "hit!" + enemyship[i].name + " destroyed";
                }
            }
        }
    }

    public void recieveenemy()
    {
        for(i=0; i<=4; i++)
        {
            enemyship[i].transform.position = empty[i].transform.position;
        }
        hitted[0] = Mathf.RoundToInt(hitsrecorder.transform.position.x);
        hitted[1] = Mathf.RoundToInt(hitsrecorder.transform.position.y);
        hitted[2] = Mathf.RoundToInt(hitsrecorder.transform.position.z);
        hitted[3] = Mathf.RoundToInt(hitsrecorder.transform.localScale.x);
        hitted[4] = Mathf.RoundToInt(hitsrecorder.transform.localScale.y);
        int a = 0;
        for (i=0; i<=4; i++)
        {
            if (hitted[i] >= health[i])
            {
                if (isLocalPlayer) 
                {
                    gameObject.transform.position = myship[i].transform.position;
                    GameObject.Find("hit").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("destroyed").GetComponent<Text>().text = "NO! "+myship[i].name+" sunk!";
                    GameObject.Find("ok text").GetComponent<Text>().text = "Okay... :(";
                    Debug.Log("number of ships destroyed: " + i);
                    a += 1;
                }
            }
        }
        shipdestroyed = a;
        if (shipdestroyed == 5)
        {
            Cmdendgame(gameObject.tag);
        }

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

    public void animate(Vector3 pos)
    {

    }

    void countrymanager()
    {
        switch (GameObject.Find("Battleships-1200x675").GetComponent<mainmenu>().selectedcountry)
        {
            case "USA":
                scoutarea = 1;
                break;
            case "ROC":
                ROCscout = 1;
                break;
            case "RPC":
                
                break;
            case "Britain":
                damage = 2;
                break;
            case "drawgrid":
                
                break;
        }
    }
    void definemyship()
    {
        myship[0] = GameObject.Find("Patrol");
        myship[1] = GameObject.Find("Destroyer");
        myship[2] = GameObject.Find("Cruiser");
        myship[3] = GameObject.Find("Battleship");
        myship[4] = GameObject.Find("Carrier");
    }
    public void setenemies(Vector3[] playershippos, int[] hits, int active)
    {
        for (i = 0; i <= 4; i++)
        {
            Debug.Log("empty: " + empty[i].transform.position);
            empty[i].transform.position = playershippos[i];
        }

        hitsrecorder.transform.position = new Vector3(hits[0], hits[1], hits[2]);
        hitsrecorder.transform.localScale = new Vector3(hits[3], hits[4], 0);
        Debug.Log("enemy and hits setted");

        GameObject.Find("record active player").transform.position = new Vector3(active, 0, 0);
    }
    [Command]
    public void Cmdsetenemy(Vector3[] playershippos, int[] hits, int active)
    {
            for (i = 0; i <= 4; i++)
            {
                Debug.Log("empty: " + empty[i].transform.position);
                empty[i].transform.position = playershippos[i];
            }

            hitsrecorder.transform.position = new Vector3(hits[0], hits[1], hits[2]);
            hitsrecorder.transform.localScale = new Vector3(hits[3], hits[4], 0);
            Debug.Log("enemy and hits setted");

            GameObject.Find("record active player").transform.position = new Vector3(active, 0, 0);
    }

    [Command]
    public void Cmdendgame(string loser)
    {
        Debug.Log("ending game");
        GameObject.Find("endgame").GetComponent <CanvasScaler>().scaleFactor = 0;
        if(loser == "1")
        {
            GameObject.Find("endtext").GetComponent<Text>().text = "The winner is player2!";
        }
        else
        {
            GameObject.Find("endtext").GetComponent<Text>().text = "The winner is player1!";
        }
        if(loser == gameObject.tag && isLocalPlayer)
        {
            GameObject.Find("loseimage").transform.localScale = new Vector3(1, 1, 1);
        }
        if (loser != gameObject.tag && isLocalPlayer)
        {
            GameObject.Find("winimage").transform.localScale = new Vector3(1, 1, 1);
        }
    }

}
