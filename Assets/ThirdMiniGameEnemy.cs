using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdMiniGameEnemy : MonoBehaviour
{
    public static ThirdMiniGameEnemy Instance;

    public Sprite checkingSprite;

    public Sprite[] idleSprites;

    bool playanim;

    [HideInInspector] public bool checking;
    SpriteRenderer spriteRenderer;

    float timeToChick;
    float coolDownTime = 6f;
    float coolDown;
    float checkingTime;
    float timer;



    private void Start()
    {
        Instance = this;
        checkingTime = ThirdMiniGame.instance.checkingTime;
        timeToChick = ThirdMiniGame.instance.timeToChick;
        coolDownTime = ThirdMiniGame.instance.coolDownTime;
        timer = timeToChick;
        spriteRenderer = GetComponent<SpriteRenderer>();
        coolDown = coolDownTime;
        playanim = true;
    }


    private void Update()
    {
        coolDownTime = Mathf.Clamp(coolDownTime, 0.2f, 5f);
        timeToChick = Mathf.Clamp(timeToChick, 0.3f, 10f);
        checkingTime = Mathf.Clamp(checkingTime, 1f, 6f);
        if (ThirdMiniGame.instance.gameIsActive)
        {
            if (playanim)
            {
                StartCoroutine(spriteAnimation());
                playanim = false;
            }
            
            if (timer <= 0)
            {
                if (coolDown <= 0)
                {
                    int r = Random.Range(0, 2);
                    if (r == 1 && !checking)
                    {
                        StopAllCoroutines();
                        spriteRenderer.sprite = checkingSprite;
                        StartCoroutine(chick());
                        coolDown = coolDownTime + checkingTime;
                        
                    }
        
                }
                
                timer = timeToChick;
            }
            else { 
                timer -= Time.deltaTime;
                coolDown -= Time.deltaTime;
            }

        }
        

        Debug.Log("cooldown time: " + coolDownTime);
        Debug.Log("time to chick: " + timeToChick);
    }


    IEnumerator chick()
    {

        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x - 0.7f, transform.position.y, transform.position.z);
        AudioManager.instance.pauseSound("third game music");
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.3f);
        checking = true;
        yield return new WaitForSeconds(Random.Range(1.5f, checkingTime));
        checking = false;
        spriteRenderer.flipX = false;
        playanim = true;
        transform.position = oldPos;
        AudioManager.instance.unPauseSound("third game music");
    }


    IEnumerator spriteAnimation()
    {
        while (true)
        {
            foreach (Sprite sprite in idleSprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(0.4f);
            }
        }
    }



}
