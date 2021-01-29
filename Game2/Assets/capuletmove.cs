using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capuletmove : MonoBehaviour
{
    public GameObject gameObjectB;
    private bool enter = true;
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite sprite2;
    public Sprite sprite3;
    static float speed;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        speed = 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = gameObjectB.transform.position;
        if (pos.x >= 105.7 && pos.x < 109 && pos.y > 5 && pos.y <=6)
        {
            if (enter== true)
            {
                StartCoroutine(your_timer());     
			}
            Vector2 capuletposition = transform.position;
            capuletposition.x = capuletposition.x + speed;
            transform.position = capuletposition;
		}
    }
     IEnumerator your_timer() 
    {
        enter = false;
        yield return new WaitForSeconds(2.5f);
        ChangeSprite(newSprite);
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        ChangeSprite(sprite2);
        speed = -0.02f;
        yield return new WaitForSeconds(0.5f);
        ChangeSprite(sprite3);

	}
     void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite; 
    }
}
