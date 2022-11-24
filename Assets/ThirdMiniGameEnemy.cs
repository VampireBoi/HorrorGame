using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdMiniGameEnemy : MonoBehaviour
{
    public static ThirdMiniGameEnemy Instance;

    [HideInInspector] public bool checking;
    SpriteRenderer spriteRenderer;

    float timeToChick;
    float checkingTime;
    float timer;



    private void Start()
    {
        Instance = this;
        checkingTime = ThirdMiniGame.instance.checkingTime;
        timeToChick = ThirdMiniGame.instance.timeToChick;
        timer = timeToChick;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    private void Update()
    {
        if (ThirdMiniGame.instance.gameIsActive)
        {
            if( timer <= 0)
            {
                int r = Random.Range(0, 2);
                if(r == 1 && !checking)
                {
                    StartCoroutine(chick());
                }

                timer = timeToChick;
            }
            else { timer -= Time.deltaTime; }
        }
    }


    IEnumerator chick()
    {
        
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.3f);
        checking = true;
        yield return new WaitForSeconds(checkingTime);       
        checking = false;
        spriteRenderer.flipX = false;

    }
}
