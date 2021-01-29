using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 
using UnityEngine.Events; 
 
public class RomeoMove : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float move_x; 
    float move_y;
    float click;
    public static bool julietfollow;
    public Transform Romeo;
    public Transform Juliet;
    string a;
    string b;
    string c;
    string d;
    string e;
    string f;
    string g;
    string h;
    public Text dialogue;
    public Text input;
    public Text timer;
    private bool check;
    int num;
    float speed;
    private static bool givemap;
    private bool enter = true;
    bool busy;
    public Camera playercamera;
    public Camera mapcamera;
    Collider2D collider1;
    void Start() 
    {
	    rigidbody2d = GetComponent<Rigidbody2D>(); 
        a ="These times of woe afford no time to woo. Madam, good night: commend me to your daughter."; //I will, and know her mind early to-morrow; To-night she is mew'd up to her heaviness.
        b ="I mean, an we be in choler, we'll draw."; //Ay, while you live, draw your neck out o' the collar.
        c = "Away, begone; the sport is at the best."; //Ay, so I fear; the more is my unrest.
        d = "Where the devil should this Romeo be? Came he not home to-night?"; //Not to his father's; I spoke with his man.
        e = "The ship, sir, the slip; can you not conceive?"; //Pardon, good Mercutio, my business was great; and in such a case as mine a man may strain courtesy.
        f = "Thou art like one of those fellows that when he enters the confines of a tavern claps me his sword upon the table and says 'God send me no need of thee!' and by the operation of the second cup draws it on the drawer, when indeed there is no need."; //Am I like such a fellow?
        g = "So shall you feel the loss, but not the friend Which you weep for."; //Feeling so the loss, Cannot choose but ever weep the friend.
        h = "Sirrah, go hire me twenty cunning cooks."; //You shall have none ill, sir; for I'll try if they can lick their fingers.
        dialogue.enabled = false;
        input.enabled = false;
        check = false;
        speed = 3.0f;
        playercamera.enabled = true;
        mapcamera.enabled = false;
        timer.enabled = false;
        busy = false;
        collider1 = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (busy == true)
        {
            GameObject Montague = GameObject.FindWithTag("Montague");
            Collider2D collider4 = Montague.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collider4, collider1); 
            GameObject capulet = GameObject.FindWithTag("Capulet");
            Collider2D collider2 = capulet.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collider2, collider1);
            GameObject priest = GameObject.FindWithTag("priest");
            if (priest)
            {
                Collider2D collider3 = priest.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(collider3, collider1);         
			}
		}
        givemap = priestmove.givemap;
        move_x = Input.GetAxis("Horizontal");
        move_y = Input.GetAxis("Vertical");
        click = Input.GetAxis("map");
        if (click > 0 && givemap == true)
        {
             playercamera.enabled = false;;
             mapcamera.enabled = true;
        }
        else if (click <= 0 )
        {
            playercamera.enabled = true;
            mapcamera.enabled = false;      
		}
        
        
    }
    void FixedUpdate()
    {
	    Vector2 romeoposition = rigidbody2d.position;
        romeoposition.x = romeoposition.x + speed * move_x * Time.deltaTime;
        romeoposition.y = romeoposition.y + speed * move_y * Time.deltaTime;
	    rigidbody2d.MovePosition(romeoposition);
        if (romeoposition.x >= -6.94 && romeoposition.x <= -6.49 && romeoposition.y >= 39.2 && romeoposition.y <= 39.8 )
        {
            julietfollow = true;
            dialogue.enabled = true;
            dialogue.text = "Romeo, my love!";
        }
        if(julietfollow)
        {
            Juliet.position = Romeo.position;
   
            Juliet.parent = Romeo;
                //var julietposition = GameObject.Find("Juliet").transform.position;
                //julietposition.x = romeoposition.x -1;
                //julietposition.y = romeoposition.y - 1;  
        }  
        if (transform.position.x >= 105.7 && transform.position.x < 109 && transform.position.y > 5 && transform.position.y <=6)
        {
            julietfollow = false;
            Juliet.parent = null; 
            speed = 0;
            if (enter==true)
            StartCoroutine(your_timer1());
		}
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Montague" && julietfollow == true)
        {
            busy = true;
            dialogue.enabled = true;
            input.enabled = true;
            speed = 0.0f;
            num = Random.Range(1,9);  
            switch (num)
            {
                case 1:
                    dialogue.text = a;
                    break;
                case 2:
                    dialogue.text = b;
                    break;
                case 3:
                    dialogue.text = c;
                    break;
                case 4:
                    dialogue.text = d;
                    break;
                case 5:
                    dialogue.text = e;
                    break;
                case 6:
                    dialogue.text = f;
                    break;
                case 7:
                    dialogue.text = g;
                    break;
                case 8:
                    dialogue.text = h;
                    break;
            }
            StartCoroutine(your_timer());
        }


		
        if (collision.gameObject.tag == "Capulet")
        {  
            busy = true;
            dialogue.enabled = true;
            input.enabled = true;
            timer.enabled = true;
            speed = 0.0f;
            num = Random.Range(1,9);  
            switch (num)
            {
                case 1:
                    dialogue.text = a;
                    break;
                case 2:
                    dialogue.text = b;
                    break;
                case 3:
                    dialogue.text = c;
                    break;
                case 4:
                    dialogue.text = d;
                    break;
                case 5:
                    dialogue.text = e;
                    break;
                case 6:
                    dialogue.text = f;
                    break;
                case 7:
                    dialogue.text = g;
                    break;
                case 8:
                    dialogue.text = h;
                    break;
            }
            StartCoroutine(your_timer());
        }
	}
    IEnumerator your_timer1() 
    {
        enter = false;
        dialogue.enabled = true;
        timer.enabled = true;
        dialogue.text = "Romeo, the love I bear thee can afford no better term than this: thou art a villain.";
        yield return new WaitForSeconds(2.5f);
        dialogue.color = Color.red;
        dialogue.text = "Father!!!";
        Juliet.position = new Vector2(106.0f, 5.5f);
	}
    IEnumerator your_timer() 
    {
        for (int i = 29; i > 0; i--) 
        {
            timer.text = (i.ToString());
            yield return new WaitForSeconds(1);
        }
        speed = 3.0f;
        switch (num)
            {
                case 1:
                    if (input.text !="I will, and know her mind early to-morrow; To-night she is mew'd up to her heaviness.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);    
                        dialogue.text = "The right line is: I will, and know her mind early to-morrow; To-night she is mew'd up to her heaviness.";
					}
                    break;
                case 2:
                    if (input.text !="Ay, while you live, draw your neck out o' the collar.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);   
                        dialogue.text = "The right line is: Ay, while you live, draw your neck out o' the collar.";
					}
                    break;
                case 3:
                    if (input.text !="Ay, so I fear; the more is my unrest.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f); 
                        dialogue.text = "The right line is: Ay, so I fear; the more is my unrest.";
					}
                    break;
                case 4:
                    if (input.text !="Not to his father's; I spoke with his man.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);    
                        dialogue.text = "The right line is: Not to his father's; I spoke with his man.";
					}
                    break;
                case 5:
                    if (input.text !="Pardon, good Mercutio, my business was great; and in such a case as mine a man may strain courtesy.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);      
                        dialogue.text = "The right line is: Pardon, good Mercutio, my business was great; and in such a case as mine a man may strain courtesy.";
					}
                    break;
                case 6:
                    if (input.text !="Am I like such a fellow?")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);    
                        dialogue.text = "The right line is: Am I like such a fellow?";
					}
                    break;
                case 7:
                    if (input.text !="Feeling so the loss, Cannot choose but ever weep the friend.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f);      
                        dialogue.text = "The right line is: Feeling so the loss, Cannot choose but ever weep the friend.";
					}
                    break;
                case 8:
                    if (input.text !="You shall have none ill, sir; for I'll try if they can lick their fingers.")
                    {
                        transform.position = new Vector2(-48.63f, 0.2f); 
                        dialogue.text = "The right line is: You shall have none ill, sir; for I'll try if they can lick their fingers.";
					}
                    break;
            }
        input.enabled = false;
        timer.enabled = false;
        yield return new WaitForSeconds(5);
        dialogue.enabled = false;
    }
}
