using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    bool chase;
    float speed;
    public static int dir_x;
    public static int dir_y;
    int dirx;
    int diry;
    public static bool wait;
    private bool enter = false;
    private bool enter1 = false;
    float pos1x;
    float pos2x;
    float pos1y;
    float pos2y;
    private static bool julietfollow;
    Collider2D collider1;
    // Start is called before the first frame update
    void Start()
    { 
        rigidbody2d = GetComponent<Rigidbody2D>(); 
        wait = false;
        collider1 = GetComponent<Collider2D>();
        GameObject capulet = GameObject.FindWithTag("Capulet");
        Collider2D collider2 = capulet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, collider1);
        GameObject priest = GameObject.FindWithTag("priest");
        Collider2D collider3 = priest.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider3, collider1);
        GameObject Montague = GameObject.FindWithTag("Montague");
        Collider2D collider4 = Montague.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider4, collider1);
    }

    // Update is called once per frame
    void Update()
    {
        collider1 = GetComponent<Collider2D>();
        GameObject capulet = GameObject.FindWithTag("Capulet");
        Collider2D collider2 = capulet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, collider1);
        GameObject priest = GameObject.FindWithTag("priest");
        if (priest)
        {
            Collider2D collider3 = priest.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collider3, collider1);
        }
        GameObject Montague = GameObject.FindWithTag("Montague");
        Collider2D collider4 = Montague.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider4, collider1);
	}
    void FixedUpdate()
    {
        collider1 = GetComponent<Collider2D>();
        GameObject capulet = GameObject.FindWithTag("Capulet");
        Collider2D collider2 = capulet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, collider1);
        GameObject priest = GameObject.FindWithTag("priest");
        if (priest)
        {
            Collider2D collider3 = priest.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collider3, collider1);
        }
        GameObject Montague = GameObject.FindWithTag("Montague");
        Collider2D collider4 = Montague.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider4, collider1);
        chase = false;
        speed = 3.0f;
        dir_x = dirx;
        dir_y = diry;
        if (wait == false)
        {
                Vector2 enemyposition = rigidbody2d.position;
                enemyposition.x = enemyposition.x +  4*dir_x * Time.deltaTime;
                enemyposition.y = enemyposition.y +  4*dir_y * Time.deltaTime;
                rigidbody2d.MovePosition(enemyposition);
        }

        else if (wait == true)
        {
            if (enter == false)
                StartCoroutine(your_timer());
	    }
    }
    IEnumerator your_timer() {
        enter = true;
        Debug.Log("Your enter Coroutine at" + Time.time);
        yield return new WaitForSeconds(30.0f);
        enter = false;
        wait = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider = collision.collider;
            julietfollow = RomeoMove.julietfollow; 

            if (collision.gameObject.tag == "Juliet")
            {
                if (julietfollow == true) 
                wait = true;         
			}
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
                wait = true;       	
            }
        }
}
