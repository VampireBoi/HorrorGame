using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDPlayerMovement : MonoBehaviour
{

    public float MovementSpeed;
    Rigidbody rb;
    Vector3 moveDirection;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = gameObject.transform.Find("mini game player").GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        processInputs();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void processInputs()
    {
        float movex = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(movex > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(movex != 0) { spriteRenderer.flipX = false; }

        moveDirection = new Vector3(movex, moveY, 0f).normalized;
    }


    void Move()
    {
        rb.velocity = new Vector3(moveDirection.x * MovementSpeed *Time.deltaTime, moveDirection.y * MovementSpeed * Time.deltaTime, moveDirection.z * MovementSpeed * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Mini game Key"))
        {
            Destroy(collision.transform.gameObject);
            MiniGame.instance.advanceLevel();
            Debug.Log("level cleard");
        }
    }

   
}
