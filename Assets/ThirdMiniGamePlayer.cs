using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdMiniGamePlayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float movex = Input.GetAxisRaw("Horizontal");
        
        if(movex < 0)
        {
            if (ThirdMiniGameEnemy.Instance.checking)
            {
                ThirdMiniGame.instance.GameOver();
            }
            transform.Translate(-transform.right * speed * Time.deltaTime);
            spriteRenderer.flipX = true;
        }
        else { spriteRenderer.flipX = false; }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "third mini game door")
        {
            ThirdMiniGame.instance.progress();
        }
    }
}
