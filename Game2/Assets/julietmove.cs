using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class julietmove : MonoBehaviour
{ 
   
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == new Vector3(106.0f, 5.5f))
        {
            StartCoroutine(your_timer());
        }    
    }
    IEnumerator your_timer()
    {
        yield return new WaitForSeconds(0.5f);
        ChangeSprite(newSprite);
    }
    void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite; 
    }
}
