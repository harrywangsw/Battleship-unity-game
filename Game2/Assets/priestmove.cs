using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class priestmove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    bool chase;
    float speed;
    public static int dir_x;
    public static int dir_y;
    int dirx;
    int diry;
    float pos1x;
    float pos2x;
    float pos1y;
    float pos2y;
    public static bool givemap;
    public Text dialogue;
    public Text timer;
    private bool talk = true;
    void Start()
    {
            rigidbody2d = GetComponent<Rigidbody2D>();   
            givemap = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        dir_x = dirx;
        dir_y = diry;
        if (givemap == false)
        {
              
		
        Vector2 enemyposition = rigidbody2d.position;
        enemyposition.x = enemyposition.x +  4*dir_x * Time.deltaTime;
        enemyposition.y = enemyposition.y +  4*dir_y * Time.deltaTime;
        rigidbody2d.MovePosition(enemyposition);
        }
        else if (givemap == true && talk == true)
        {
            talk = false;
            dialogue.enabled = true;
            timer.enabled = true;
            dialogue.text = "My goodness. It is thou, Romeo. I see on thou face that thou business is serious. Utter no more of this. I shall aide you the best I can for it is dangerous to go alone. Here, take this.";
		    timer.text = "You got the map of the maze! Press the m key to view it.";
            Destroy(gameObject,2.5f);
            StartCoroutine(your_timer());
        }


    }
    IEnumerator your_timer() {
        yield return new WaitForSeconds(2.5f);
        dialogue.enabled = false;
        timer.enabled = false;
    }
        
    void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider = collision.collider;
            if (collision.gameObject.tag == "Walls")
            {
                Vector2 contactPoint = collision.contacts[0].point;
                Vector2 center = transform.position;
                bool right = contactPoint.x > center.x;
  
                bool top = contactPoint.y > center.y;
                bool left = contactPoint.x < center.x;
                bool bottom = contactPoint.y < center.y;
                float f = contactPoint.y - center.y;
                float f1 = contactPoint.x - center.x;
                if (right == true && Mathf.Abs(f)<Mathf.Abs(f1))
                {

                    transform.Translate(-0.05f, 0.0f, 0.0f);
                    dirx = 0;
                    var rand = Random.Range(1,3);
                    if (rand == 1)
                        diry = 1;
                    else if (rand == 2) 
                    
                        diry = -1;
   
                }
                if (left == true && Mathf.Abs(f)<Mathf.Abs(f1))
                {
                    transform.Translate(0.05f, 0.0f, 0.0f);
                    dirx = 0;
                    var rand = Random.Range(1,3);
                    if (rand == 1)
                        diry = 1;
                    else if (rand == 2) 
                    
                        diry = -1;
                        
                }
                if (top == true && Mathf.Abs(f)>Mathf.Abs(f1))
                {
                    transform.Translate(0.0f, -0.05f, 0.0f);
                    diry = 0;
                    var rand = Random.Range(1,3);
                    if (rand == 1) 
                        dirx = 1;
                    else if (rand == 2) 
                    
                        dirx = -1;
                    
                }
                if (bottom == true && Mathf.Abs(f)>Mathf.Abs(f1))
                {
                    transform.Translate(0.0f, 0.05f, 0.0f);
                    diry = 0;
                    var rand = Random.Range(1,3);
                    if (rand == 1) 
                        dirx = 1;
                    else if (rand == 2) 
                    
                        dirx = -1;
                    
                }
            }
            if (collision.gameObject.tag == "Romeo")
            {
                givemap = true;
            }
        }
}
