using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goback : MonoBehaviour
{
    float click;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	click = Input.GetAxis("map");
        if (click <= 0)
        {
             SceneManager.LoadScene("Mainscene", LoadSceneMode.Single);
        }    
    }
}
