using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class FirstMiniGame : MonoBehaviour
{
    public static FirstMiniGame instance;

    
    [HideInInspector] public bool gameIsFinished;

    [Header("the time to load the next level \"in secends\"")]
    public float TimeToLoadTheNextLevel;

    public GameObject levelManager;

    [Range(0.3f, 2f)] public float difficulty;
    public float timeToFinichTheGame;
    public int levelsInDisk;

    //[HideInInspector]
    //public GameObject floppyDiskInHand;
    
    int levelCounter;

    //public GameObject titleScreen;

    Material crtMat;
    Material GiltchMat;

    GameObject computerScreen;

    float timerGiltchingFreq;

    //bool startgame;
    bool a;
    
    GameObject MiniGameCam;
    Dialogue dialogue;
    // the desk inside the computer;
    //[HideInInspector]
    //public FloppyDisk currentDesk;

    //to check 
    //public bool thereIsFloopyDesk;

    //public RenderTexture screen;
    bool playDialogue;

    //public Sprite loading;
    //public Sprite closing;

    //public GameObject text;


    GameObject scoreUI;
    GameObject timerUI;




    float timer = 0;



    bool countTime = false;
    bool isLoading = false;

    bool playSound;

    //when the timer runs out and the computer makes the beb sound
    //[HideInInspector] public bool alertMode;


    //the light coming from the screen when the game is running "change later but works for now" 
    
    GameObject tvLight;
    GameObject tvloadingScreen;

    private void Start()
    {

        gameIsFinished = false;
        
        scoreUI = Computer.instance.scoreUI;
        timerUI = Computer.instance.timerUI;

        MiniGameCam = Computer.instance.MiniGameCam;
        GiltchMat = Computer.instance.GiltchMat;
        crtMat = Computer.instance.crtMat;
        computerScreen = Computer.instance.computerScreen;
        dialogue = Computer.instance.dialogue;
        
        tvloadingScreen = Computer.instance.tvloadingScreen;
        tvLight = Computer.instance.tvLight;
        Invoke("spawnLevel", 0.1f);

        startMiniGame();
        timerGiltchingFreq = 500;       
        //timerUI = ui.transform.Find("Timer").gameObject;
        playDialogue = true;     
        instance = this;
        playSound = true;

        isLoading = true;
        countTime = true;
        timer = timeToFinichTheGame;
    }


    private void Update()
    {
        

        if (Computer.instance.thereIsFloopyDesk && levelCounter >= levelsInDisk)
        {
            countTime = false;
            winTheGame();
        }



        if (MiniGameCam.activeSelf && GameObject.FindGameObjectWithTag("2dPlayer") != null)
        {
            MiniGameCam.transform.position = GameObject.FindGameObjectWithTag("2dPlayer").transform.position;
            MiniGameCam.transform.position = new Vector3(MiniGameCam.transform.position.x, MiniGameCam.transform.position.y, -1);
        }


    
        scoreUI.gameObject.SetActive(true);
        if (scoreUI.activeSelf)
        {         
            scoreUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "level: " + (levelCounter + 1).ToString() + " out of: " + levelsInDisk.ToString();
        }

        if (Computer.instance.currentDesk != null)
        {

            if (isLoading)
            {
                turnOffGameUI();
            }
            else if (!dialogue.dialogueOn)
            {
                turnOnGameUI();
            }
            // when the game is loading or in dailaogue is hides the ui and lock the player 


            if (a == true && !dialogue.dialogueOn)
            {

                Debug.Log("a dialouge has been ended");
                turnOnGameUI();
                GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
                if (p != null)
                {
                    p.GetComponent<TwoDPlayerMovement>().CanWalk = true;
                }
                
                
                if (Computer.instance.currentDesk.firstInsertion)
                {
                    AudioManager.instance.playSound("mini game background music ");
                    AudioManager.instance.playSound("timer sound");
                    StartCoroutine(timerGiltching());
                    countTime = true;
                    Computer.instance.currentDesk.firstInsertion = false;
                    Enemy.instanse.canChick = true;
                }

                a = false;
            }
            if (dialogue.dialogueOn && a == false)
            {
                countTime = false;
                Debug.Log("a dialouge has been started");
                turnOffGameUI();
                GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
                if (p != null)
                {
                    p.GetComponent<TwoDPlayerMovement>().CanWalk = false;
                }
                a = true;
            }

        }


        if (timerUI.activeSelf && countTime && Computer.instance.plugedIn)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time:" + (int)timer + " s";
                timerSound(timer);
            }
            else
            {
                //here we trigger the timeout event
                AudioManager.instance.stopSound("timer sound");
                AudioManager.instance.stopSound("mini game background music ");
                GameOver();

            }
        }
     
    }

    


    //this code is hard coded , adjust it later 
    //this is hard coded 
    



    // adjust the computer mode from here
    void startMiniGame()
    {
        isLoading = true;
        countTime = false;
        timer = timeToFinichTheGame;
        MiniGameCam.SetActive(true);
        tvLight.SetActive(true);
        
    }

    public void spawnLevel()
    {
       Instantiate(levelManager, gameObject.transform).GetComponent<LevelManager>();

        //for controlling the ui       
        countTime = false;
        isLoading = false;

        tvloadingScreen.SetActive(false);

        //tvLight.GetComponent<Light>().color = Computer.instance.currentDesk.disk.screenLightColor;

        if (Computer.instance.currentDesk.firstInsertion)
        {
            AudioManager.instance.playSound("mini game background music ");
           

            Invoke("startFirstDialouge", 2f);

        }
        else
        {
            AudioManager.instance.playSound("mini game background music ");
            countTime = true;
            AudioManager.instance.playSound("timer sound");
            StartCoroutine(timerGiltching());
            turnOnGameUI();
        }

    }

    public void advanceLevel()
    {

        //for controlling the ui
        isLoading = true;
        countTime = false;

        levelCounter++;
        if (levelCounter < levelsInDisk)
        {
            LevelManager.instance.destroyCurrentLevel();
            turnOffGameUI();
            tvloadingScreen.SetActive(true);
            Invoke("spawnLevel", TimeToLoadTheNextLevel);
        }

    }


    void turnOffGameUI()
    {
        scoreUI.SetActive(false);
        timerUI.SetActive(false);
    }
    void turnOnGameUI()
    {       
        scoreUI.SetActive(true);
        timerUI.SetActive(true);
    }



    void destoryMinigame()
    {
        StopAllCoroutines();

        countTime = false;
        tvloadingScreen.SetActive(false);
        AudioManager.instance.stopSound("glitch sound");
        Destroy(gameObject);


    }

    //destroy the level that you were playing when you exit 
    void closeMiniGameLevel()
    {
        levelCounter = 0;

        Object[] t = GameObject.FindObjectsOfType(typeof(LevelManager));
        if (t.Length > 0)
        {
            LevelManager.instance.destroyCurrentLevel();
        }
    }

    public void closeMiniGame()
    {
        turnOffGameUI();
        AudioManager.instance.stopSound("mini game background music ");
        AudioManager.instance.stopSound("timer sound");
        AudioManager.instance.stopSound("glitch sound");
        Invoke("closeMiniGameLevel", 0.3f);
        Invoke("destoryMinigame", 0.5f);     
    }



     
    void winTheGame()
    {
        if (playDialogue)
        {
            StopAllCoroutines();
            timerGiltchingFreq = 1000;
            AudioManager.instance.stopSound("timer sound");
            AudioManager.instance.stopSound("mini game background music ");         
            dialogue.startDialogue(Computer.instance.currentDesk.LastDialogue);

            playDialogue = false;
        }

        if (!dialogue.dialogueOn)
        {
            a = false;          
            playDialogue = true;
            levelCounter = 0;  
            gameIsFinished = true;
        }
    }

    void startFirstDialouge()
    {
        turnOffGameUI();
        AudioManager.instance.stopSound("mini game background music ");

        dialogue.startDialogue(Computer.instance.currentDesk.firstDialogue);
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


    void GameOver()
    {
        StartCoroutine(triggerAlert());
    }
    IEnumerator triggerAlert()
    {
        Computer.instance.inAlertMode = true;
        turnOffGameUI();
        AudioManager.instance.stopSound("mini game background music ");


        GameObject T = GameObject.Find("2dplayer(Clone)");
        if (T != null)
        {
            T.GetComponent<TwoDPlayerMovement>().CanWalk = false;
        }
        yield return new WaitForSeconds(1.5f);
        if (playSound)
        {
            AudioManager.instance.playSound("alert sound");
            playSound = false;
        }
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
