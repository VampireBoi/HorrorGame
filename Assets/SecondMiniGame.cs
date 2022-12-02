using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecondMiniGame : MonoBehaviour
{
    public static SecondMiniGame instance; 

    public GameObject inviroment;
    public GameObject miniGamePlayer;
    GameObject i;
    GameObject m;


    [HideInInspector] public bool gameIsFinished;

    // for the game -------------------------------------------

    [HideInInspector] public bool gameIsActive;


    public Transform[] spwanPos;
    public GameObject fireBall;
    public float timeForRound;
    
    
    [Header("the projectiles inital speed")]

    public float projectileSpeed;

    [Header("the least projectiles speed")]
    public float projectileSpeedLimit;


    [Header("the inital time between each projectile to spawn")]
    public float timeTospwan;


    [Header("the least amount of time between each projectile to spawn")]
    public float timeToSpownLimit;
    
    
    public int roundsToWin;

    [Header("diffeculity increases each round progress")]
    public float timeToSpawnDecreasedPerRound;
    public float projectileSpeedIncreasedPerRound;
    float spawnTimer; 
    float roundTimer;

    int roundCounter;

    // for the computer-----------------------------------------
    int levelCounter;

    //public GameObject titleScreen;
    Material crtMat;
    Material GiltchMat;
    GameObject computerScreen;
    float timerGiltchingFreq;
    bool startgame;
    GameObject MiniGameCam;
    Dialogue dialogue; 
    bool playDialogue;
    
    GameObject roundUI;
    GameObject scoreUI;
    GameObject timerUI;
    
    bool a;



    bool countTime = false;
    bool isLoading = false;
    bool playSound;

    //when the timer runs out and the computer makes the beb sound
    //[HideInInspector] public bool alertMode;


    //the light coming from the screen when the game is running "change later but works for now" 

    GameObject tvLight;
    GameObject tvloadingScreen;




    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameIsActive = false;
        gameIsFinished = false;

        roundUI = Computer.instance.ui.transform.Find("second game ui").gameObject;
        scoreUI = Computer.instance.scoreUI;
        timerUI = Computer.instance.timerUI;

        MiniGameCam = Computer.instance.MiniGameCam;
        GiltchMat = Computer.instance.GiltchMat;
        crtMat = Computer.instance.crtMat;
        computerScreen = Computer.instance.computerScreen;
        dialogue = Computer.instance.dialogue;

        tvloadingScreen = Computer.instance.tvloadingScreen;
        tvLight = Computer.instance.tvLight;

        playDialogue = true;

        
        spawnTimer = timeTospwan;
        roundTimer = timeForRound;

        spawnLevel();
        turnOffGameUI();
        MiniGameCam.transform.position = new Vector3(m.transform.position.x, m.transform.position.y - 1f, m.transform.position.z - 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        //ui display
        if (roundUI.activeSelf)
        {
            roundUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ROUND: " + roundCounter;
        }




        // when the game is loading or in dailaogue is hides the ui and lock the player 

        if (a == true && !dialogue.dialogueOn)
        {

            Debug.Log("a dialouge has been ended");
            //turnOnGameUI();
            AudioManager.instance.playSound("second game background music");

            GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
            if (p != null)
            {
                p.GetComponent<SecondMiniGamePlayer>().canMove = true;
            }


            if (Computer.instance.currentDesk.firstInsertion)
            {
                //AudioManager.instance.playSound("mini game background music ");
                //AudioManager.instance.playSound("timer sound");
                //StartCoroutine(timerGiltching());
                //countTime = true;
                Computer.instance.currentDesk.firstInsertion = false;
            }

            turnOnGameUI();
            gameIsActive = true;
            a = false;
        }
        if (dialogue.dialogueOn && a == false)
        {
            Debug.Log("a dialouge has been started");
            AudioManager.instance.stopSound("second game background music");


            GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
            if (p != null)
            {
                p.GetComponent<SecondMiniGamePlayer>().canMove = false;
            }

            turnOffGameUI();
            gameIsActive = false;
            a = true;
        }
      

        if (gameIsActive)
        {
            if(roundCounter < roundsToWin)
            {
                if (timeTospwan < timeToSpownLimit)
                {
                    timeTospwan = timeToSpownLimit;
                }
                if(projectileSpeed > projectileSpeedLimit)
                {
                    projectileSpeed = projectileSpeedLimit;
                }

                if (roundTimer <= 0)
                {
                    StartCoroutine(progressRound());
                    if (timeTospwan > timeToSpownLimit)
                    {
                        timeTospwan -= timeToSpawnDecreasedPerRound;
                    }

                    if(projectileSpeed < projectileSpeedLimit)
                    {
                        projectileSpeed += projectileSpeedIncreasedPerRound;
                    }
                    roundTimer = timeForRound;
                }
                else
                {
                    roundTimer -= Time.deltaTime;
                    if (spawnTimer <= 0)
                    {
                        //spwan fireball
                        int r = Random.Range(0, spwanPos.Length);
                        GameObject f = Instantiate(fireBall, spwanPos[r].localPosition, Quaternion.identity);

                        if (r == 0)
                        {
                            f.GetComponent<FireBall>().right = true;
                        }
                        else if (r == 1)
                        {
                            f.GetComponent<FireBall>().left = true;
                        }
                        else if (r == 2)
                        {
                            f.GetComponent<FireBall>().down = true;

                        }
                        else if (r == 3)
                        {
                            f.GetComponent<FireBall>().up = true;
                        }
                        f.GetComponent<FireBall>().speed = projectileSpeed;

                        spawnTimer = timeTospwan;
                    }
                    else { spawnTimer -= Time.deltaTime; }
                }
            }
            else
            {
                winTheGame();
            }
        }



        
                    
    }

    public void spawnLevel()
    {
        i = Instantiate(inviroment);
        m = Instantiate(miniGamePlayer);

        //for controlling the ui       
        countTime = false;
        isLoading = false;

        tvloadingScreen.SetActive(false);

        //tvLight.GetComponent<Light>().color = Computer.instance.currentDesk.disk.screenLightColor;

        if (Computer.instance.currentDesk.firstInsertion)
        {
            Invoke("startFirstDialouge", 2f);
        }
        else
        {
            //StartCoroutine(timerGiltching());  
            gameIsActive = true;
            
        }
        AudioManager.instance.playSound("second game background music");
        AudioManager.instance.changePitch("second game background music", 1f);
        Invoke("turnOnGameUI", 0.3f);

    }


    IEnumerator progressRound()
    {
        Color c;
        TextMeshProUGUI t;
        gameIsActive = false;
        GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
        if (p != null)
        {
            //p.GetComponent<SecondMiniGamePlayer>().canMove = false;
        }
        AudioManager.instance.stopSound("second game background music");
        AudioManager.instance.playSound("second game round progress");

        if (roundUI.activeSelf)
        {
            t = roundUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            c = t.color;
            t.color = Color.white;

            t.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            
            t.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);

            StartCoroutine(startGlitch(Random.Range(0f, 0.4f)));

            t.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            t.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.4f);
            roundCounter++;
            yield return new WaitForSeconds(0.4f);
            t.color = c;
               
            gameIsActive = true;
            if (p != null)
            {
                //p.GetComponent<SecondMiniGamePlayer>().canMove = true;
            }
            AudioManager.instance.playSound("second game background music");
            AudioManager.instance.speedSound("second game background music", 0.3f);
        }



    }




    void turnOffGameUI()
    {
        scoreUI.SetActive(false);
        timerUI.SetActive(false);
        roundUI.SetActive(false);
    }
    void turnOnGameUI()
    {
        roundUI.SetActive(true);
    }

    void startFirstDialouge()
    {
        turnOffGameUI();
        AudioManager.instance.stopSound("mini game background music ");
        dialogue.startDialogue(Computer.instance.currentDesk.firstDialogue);
    }



    public void closeMiniGame()
    {
        StopAllCoroutines();
        turnOffGameUI();
        AudioManager.instance.stopSound("second game background music");
        Destroy(i);
        Destroy(m);         
        Destroy(gameObject);
    }



    // anmiation and effects functions

    public void glitchScreen(float time)
    {
        StartCoroutine(startGlitch(time));
    }


    IEnumerator startGlitch(float time)
    {
        AudioManager.instance.playSound("glitch sound");
        computerScreen.GetComponent<MeshRenderer>().material = GiltchMat;
        yield return new WaitForSeconds(time);
        AudioManager.instance.stopSound("glitch sound");
        computerScreen.GetComponent<MeshRenderer>().material = crtMat;
    }


    void winTheGame()
    {
        if (playDialogue)
        {
            StopAllCoroutines();
            timerGiltchingFreq = 1000;       
            AudioManager.instance.stopSound("second game background music");
            
            dialogue.startDialogue(Computer.instance.currentDesk.LastDialogue);
            playDialogue = false;
        }

        if (!dialogue.dialogueOn)
        {
            playDialogue = true;
            gameIsFinished = true;
        }
    }


    public void GameOver()
    {
        StartCoroutine(triggerAlert());
    }
    IEnumerator triggerAlert()
    {
        Computer.instance.inAlertMode = true;
        turnOffGameUI();
        gameIsActive = false;
        AudioManager.instance.stopSound("second game background music");


        GameObject T = GameObject.FindGameObjectWithTag("2dPlayer");
        if (T != null)
        {
            T.GetComponent<SecondMiniGamePlayer>().canMove = false;
        }
        yield return new WaitForSeconds(3f);
        
        AudioManager.instance.playSound("alert sound");
            
        
        computerScreen.GetComponent<MeshRenderer>().material = GiltchMat;

        yield return new WaitForSeconds(0.8f);

        Computer.instance.alertMode = true;

        TheFirstPerson.FPSController.instance.movementEnabled = true;

        Debug.Log("you falied beebb beeeb beeeb");
    }


    IEnumerator timerGiltching()
    {
        while (true)
        {
            yield return new WaitForSeconds(timerGiltchingFreq);

            int i = Random.Range(1, 2);
            if (i == 1)
            {
                glitchScreen(Random.Range(0f, 0.3f));
            }

        }
    }

    void timerSound(float time)
    {

        if (time > 30)
        {
            timerGiltchingFreq = 20f;
            AudioManager.instance.changePitch("timer sound", 0.65f);
        }
        else if (time < 30 && time > 20)
        {
            timerGiltchingFreq = 10f;
            AudioManager.instance.changePitch("timer sound", 1f);
        }
        else if (time < 20 && time > 10)
        {
            timerGiltchingFreq = 5f;
            AudioManager.instance.changePitch("timer sound", 1.5f);
        }
        else if (time < 10 && time > 5)
        {
            timerGiltchingFreq = 3f;
            AudioManager.instance.changePitch("timer sound", 2f);
        }
        else if (time < 5 && time > 0)
        {
            timerGiltchingFreq = 1f;
            AudioManager.instance.changePitch("timer sound", 2.5f);
        }
    }


}
