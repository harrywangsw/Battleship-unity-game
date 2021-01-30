using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class click : MonoBehaviour
{
    GameObject localplayer, otherplayer;
    int[] c = new int[5];
    int i;
    public Vector3[] playerpos = new Vector3[5];
    void Start()
    {
        
    }

    void Update()
    {
        if (GameObject.Find("record active player").transform.position.x == 2.0f)
        {
            localplayer = GameObject.FindWithTag("2");
            if (localplayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GameObject[] a = localplayer.GetComponent<drawgrid>().myship;
                int[] b = localplayer.GetComponent<drawgrid>().hits;
                for (i = 0; i <= 4; i++)
                {
                    Debug.Log(a[i].transform.position);
                    playerpos[i] = a[i].transform.position;
                }

                gameObject.GetComponent<Button>().onClick.AddListener(() => localplayer.GetComponent<drawgrid>().setenemies(playerpos, b, 1));
                Debug.Log("changed active player: " + GameObject.Find("record active player").transform.position.x);
            }
        }
        else
        {
            localplayer = GameObject.FindWithTag("1");
            GameObject[] a = localplayer.GetComponent<drawgrid>().myship;
            int[] b = localplayer.GetComponent<drawgrid>().hits;
            for (i = 0; i <= 4; i++)
            {
                Debug.Log(a[i].transform.position);
                playerpos[i] = a[i].transform.position;
            }

            gameObject.GetComponent<Button>().onClick.AddListener(() => localplayer.GetComponent<drawgrid>().Cmdsetenemy(playerpos,b, 2));
            Debug.Log("changed active player: " + GameObject.Find("record active player").transform.position.x);
        }
    }

}
