using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMiniGamePlayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [HideInInspector] public bool canMove;
    public Sprite[] sprites;

    Sprite currentState;

    public GameObject[] hitBoxes;

    bool setState;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        
        setState = true;


        currentState = sprites[0];

        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (setState)
            {
                spriteRenderer.sprite = currentState;
                setState = false;
            }

            if (Computer.instance.isUsingComputer)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    hitBoxes[0].gameObject.SetActive(true);
                    hitBoxes[1].gameObject.SetActive(false);
                    hitBoxes[2].gameObject.SetActive(false);
                    spriteRenderer.sprite = sprites[2];

                    currentState = spriteRenderer.sprite;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    hitBoxes[0].gameObject.SetActive(false);
                    hitBoxes[1].gameObject.SetActive(true);
                    hitBoxes[2].gameObject.SetActive(false);
                    spriteRenderer.sprite = sprites[1];

                    currentState = spriteRenderer.sprite;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    hitBoxes[0].gameObject.SetActive(false);
                    hitBoxes[1].gameObject.SetActive(false);
                    hitBoxes[2].gameObject.SetActive(true);
                    spriteRenderer.sprite = sprites[0];

                    currentState = spriteRenderer.sprite;
                    transform.localScale = new Vector3(-15, 15, 15);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    hitBoxes[0].gameObject.SetActive(false);
                    hitBoxes[1].gameObject.SetActive(false);
                    hitBoxes[2].gameObject.SetActive(true);

                    spriteRenderer.sprite = sprites[0];

                    currentState = spriteRenderer.sprite;
                    transform.localScale = new Vector3(15, 15, 15);
                }
            }
        }
        else { 
            spriteRenderer.sprite = sprites[1];
            setState = true;
        }
        

    }
}
