using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public TextMeshProUGUI timerUI;

    bool gameOver;
    [Header("all the items that are going to be disabeld when the timer runs out")]
    public GameObject[] gameObjects;

    public Dialogue[] gameDialogues;
    [Header("timer in seconds")]
    public float time;


    float timer;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        timer = time;
        Invoke("startClockSound", 0.1f);      
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameDialogues[0].dialogueOn && !gameDialogues[1].dialogueOn)
        {

            if (timer > 60f)
            {
                float t = timer / 60f;
                timerUI.text = "TIME: " + ((int)t).ToString() + "m";
            }
            else
            {
                timerUI.text = "TIME: " + ((int)timer).ToString() + "s";
            }

            if (timer <= 0f)
            {
                // the timer is finished, kill the player
                // play scary sounds and turn off lights;

                if (!gameOver)
                {

                    if (MiniGame.instance.isUsingComputer)
                    {
                        MiniGame.instance.turnOffComputer();
                    }
                    foreach (GameObject go in gameObjects)
                    {
                        go.SetActive(false);
                    }
                    AudioManager.instance.stopAllSounds();
                    AudioManager.instance.playSound("time out jumpscare sound");
                    AudioManager.instance.stopSound("game timer");
                    timerUI.gameObject.SetActive(false);
                    Invoke("killThePlayer", 5.5f);
                    gameOver = true;
                }

            }
            else
            {
                // check if the game is on dialuoge or not;
                timer -= Time.deltaTime;

            }
        }


        
    }

    void startClockSound()
    {
        Interact.Instance.enterSaveArea();
        AudioManager.instance.playSound("game timer");
    }

    void killThePlayer()
    {
        Enemy.instanse.attackThePlayer();
    }
}
