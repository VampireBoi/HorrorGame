using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    // Start is called before the first frame update

    private void Start()
    {
        AudioManager.instance.playSound("fireball shoot");
    }

    private void Update()
    {
        if (!SecondMiniGame.instance.gameIsActive)
        {
            Destroy(gameObject);
        }

        if (left)
        {
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }
        else if(right)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else if (up)
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
        else if (down)
        {
            transform.Translate(-transform.up * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "2dPlayer")
        {
            SecondMiniGame.instance.GameOver();
            AudioManager.instance.playSound("player hit");
            Debug.Log("killled");       
        }
        else
        {
            AudioManager.instance.playSound("fireball blocked");         
        }
        Destroy(gameObject);
    }

}
