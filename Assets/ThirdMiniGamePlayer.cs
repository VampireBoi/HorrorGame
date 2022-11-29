using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdMiniGamePlayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public Sprite idle;


    bool playanim;
    public float speed;
    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        playanim = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        float movex = Input.GetAxisRaw("Horizontal");

        if (canMove)
        {
            if (movex < 0)
            {
                if (playanim)
                {
                    StartCoroutine(spriteAnimation());
                    playanim = false;
                }
                if (ThirdMiniGameEnemy.Instance.checking)
                {
                    ThirdMiniGame.instance.GameOver();
                }
                transform.Translate(-transform.right * speed * Time.deltaTime);
                spriteRenderer.flipX = false;
            }
            else 
            {   
                spriteRenderer.flipX = true;
                playanim = true;
                StopAllCoroutines();
                spriteRenderer.sprite = sprites[0];

            }
        }
        else
        {
            spriteRenderer.sprite = idle;
        }
        
    }


    IEnumerator spriteAnimation()
    {
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(0.06f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "third mini game door")
        {
            ThirdMiniGame.instance.progress();
        }
    }
}
