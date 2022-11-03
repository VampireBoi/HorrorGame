using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDPlayerMovement : MonoBehaviour
{

    public float MovementSpeed;
    Rigidbody rb;
    Vector3 moveDirection;
    SpriteRenderer spriteRenderer;

    [HideInInspector] public bool CanWalk;

    public Sprite idile;
    
    public Sprite stare;

    public Sprite[] sprites;
    bool playAnim;

    private void Awake()
    {
        CanWalk = true;     
    }
    private void Start()
    {     
        playAnim = true;
        rb = GetComponent<Rigidbody>();
        spriteRenderer = gameObject.transform.Find("mini game player").GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (CanWalk)
        {
            processInputs();
        } else
        {
            rb.velocity = Vector3.zero;
            StopAllCoroutines();
            spriteRenderer.sprite = stare;

        }
       
    }
    private void FixedUpdate()
    {
        
        Move();
         
    }

    void processInputs()
    {
        float movex = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (movex != 0 || moveY != 0)
        {
            if (playAnim)
            {
                
                StartCoroutine(spriteAnimation());
                playAnim = false;
            }       
        }
        else {
            if (!playAnim)
            {
                StopAllCoroutines();
                playAnim=true;
            }            
            spriteRenderer.sprite = idile;
        }

        if(movex > 0)
        {        
            spriteRenderer.flipX = false;
        }
        else if(movex != 0) { spriteRenderer.flipX = true; }

        
        moveDirection = new Vector3(movex, moveY, 0f).normalized;
    }


    void Move()
    {
        if (CanWalk)
        {
            rb.velocity = new Vector3(moveDirection.x * MovementSpeed * Time.deltaTime, moveDirection.y * MovementSpeed * Time.deltaTime, moveDirection.z * MovementSpeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Mini game Key"))
        {
            AudioManager.instance.playSound("key pickup");
           
            Destroy(collision.transform.gameObject);
            FirstMiniGame.instance.advanceLevel();
            
        }
    }
    IEnumerator spriteAnimation()
    {
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }




   
}
