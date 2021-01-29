using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkdialouge : MonoBehaviour
{
    string a;
    GameObject text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
        a = GetComponent<Text>().text;
        if (a == "abc")
        {
        Debug.Log("aaaaaaaaaaaaaa");
        }
    }
}
