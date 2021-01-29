using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class click : MonoBehaviour
{
    GameObject localplayer, otherplayer;
    int[] c = new int[5];
    int i;
    void Start()
    {
        
    }

    void Update()
    {
        if (GameObject.Find("record active player").transform.position.x == 2.0f)
        {
            localplayer = GameObject.FindWithTag("2");
            GameObject[] a = localplayer.GetComponent<drawgrid>().myship;
            int[] b = localplayer.GetComponent<drawgrid>().hits;

            gameObject.GetComponent<Button>().onClick.AddListener(() => localplayer.GetComponent<drawgrid>().setenemy(a,b));
            Debug.Log("changed active player: " + GameObject.Find("record active player").transform.position.x);
        }
        else
        {
            localplayer = GameObject.FindWithTag("1");
            GameObject[] a = localplayer.GetComponent<drawgrid>().myship;
            int[] b = localplayer.GetComponent<drawgrid>().hits;

            gameObject.GetComponent<Button>().onClick.AddListener(() => localplayer.GetComponent<drawgrid>().setenemy(a,b));
            Debug.Log("changed active player: " + GameObject.Find("record active player").transform.position.x);
        }
    }
    public void changeactive()
    {
        if(GameObject.Find("record active player").transform.position.x == 2.0f)
        {
            GameObject.Find("record active player").transform.position = new Vector3(1, 0, 0);
        }
        else
        {
            GameObject.Find("record active player").transform.position = new Vector3(2, 0, 0);
        }
    }

}
