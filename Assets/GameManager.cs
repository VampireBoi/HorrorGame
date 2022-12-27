using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    
    public TextMeshProUGUI timerUI;


    public int level;


    bool gameOver;
    [Header("all the items that are going to be disabeld when the timer runs out")]
    public GameObject[] gameObjects;

    public Dialogue[] gameDialogues;
    [Header("timer in seconds")]
    public float time;
    float timer;



    [Header("level two checking")]
    bool canChickOnPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
        level = 1;
        Instance = this;
        gameOver = false;
        timer = time;
        Invoke("startClockSound", 0.1f);   
        canChickOnPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O)){
            //level++;
        //}

        //Debug.Log("game level: " + level);
        if (!gameDialogues[0].dialogueOn && !gameDialogues[1].dialogueOn)
        {

            timerUI.gameObject.SetActive(true);
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

                    if (Computer.instance.isUsingComputer)
                    {
                        Computer.instance.turnOffComputer();
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
        else
        {
            timerUI.gameObject.SetActive(false);
            

        }


        if(level == 2)
        {
            if (Computer.instance.isUsingComputer)
            {
                if (Enemy.instanse.canChick && !Enemy.instanse.isCheckingNoise)
                {
                    Enemy.instanse.gocheckTheRoom();                   
                }
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


    public void resetChecking()
    {
        StartCoroutine(resetCheck());
    }
    IEnumerator resetCheck()
    {
        yield return new WaitForSeconds(15f);
        canChickOnPlayer = true;
    }
}
