using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class MiniGame : MonoBehaviour
{
    public static MiniGame instance;
    
    [Header("the time to load the next level \"in secends\"")]
    public float TimeToLoadTheNextLevel;

    public GameObject ui;
  
    [HideInInspector]
    public GameObject floppyDiskInHand; 
    public int levelCounter;

    public GameObject titleScreen;

    public Material crtMat;
    public Material GiltchMat;

    float timerGiltchingFreq;

    public GameObject computerScreen;

    bool startgame;

    bool a;
    bool inAlertMode;

    public GameObject MiniGameCam;
    
    public GameObject computerView;

    public Dialogue dialogue;
    // the desk inside the computer;
    [HideInInspector]
    public FloppyDisk currentDesk;

    //to check 
    public bool thereIsFloopyDesk;
    
    public RenderTexture screen;

    bool playDialogue;

    public Sprite loading;
    public Sprite closing;

    public GameObject text;

    [HideInInspector]public bool isUsingComputer = false;

    public GameObject scoreUI;
    public GameObject timerUI;

    public GameObject[] bootUpScreen;


    float timeToFinichTheGame;
    float timer = 0;

    public bool plugedIn = false;


    bool countTime = false;
    bool isLoading = false;

    bool playSound;

    //when the timer runs out and the computer makes the beb sound
    [HideInInspector] public bool alertMode;


    //the light coming from the screen when the game is running "change later but works for now" 
    public GameObject tvLight;
    public GameObject tvloadingScreen;

    private void Start()
    {
        timerGiltchingFreq = 500;
        startgame = false;
        scoreUI = ui.transform.Find("score background").gameObject;
        //timerUI = ui.transform.Find("Timer").gameObject;
        playDialogue = true;
        computerView.SetActive(false);
        MiniGameCam.SetActive(false);
        thereIsFloopyDesk = false;
        instance = this;
        tvLight.SetActive(false);
        tvloadingScreen.SetActive(false);
        playSound = true;
    }


    private void Update()
    {


        Debug.Log("is using the coumputer: " + isUsingComputer);
        Debug.Log("in alert mode: " + inAlertMode);
        if (MiniGameCam.activeSelf && GameObject.FindGameObjectWithTag("2dPlayer") != null)
        {
            MiniGameCam.transform.position = GameObject.FindGameObjectWithTag("2dPlayer").transform.position;
            MiniGameCam.transform.position = new Vector3(MiniGameCam.transform.position.x, MiniGameCam.transform.position.y, -1);
        }   
        //to exit the computer mode
        if (Input.GetKeyDown(KeyCode.G) && isUsingComputer && !inAlertMode)
        {
            turnOffComputer();
        }  

        //finishing the desk in take it out of the system;
        if(thereIsFloopyDesk && levelCounter >= currentDesk.disk.levelsInDesk)
        {
            countTime = false;
            takeOutCurrentFloppyDisk();
        }

        
        if(!startgame && Input.GetKeyDown(KeyCode.Return) && isUsingComputer && titleScreen.activeSelf)
        {
            startgame = true;         
        }
        
        if (startgame)
        {
            timerGiltchingFreq = 500;
            StopAllCoroutines();
            titleScreen.SetActive(false);
            Invoke("spawnLevel", 0.2f);
            startgame = false;
        }
    

        if(currentDesk != null && scoreUI.gameObject.activeSelf)
        {
            scoreUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "level: " + (levelCounter + 1).ToString() + " out of: " + currentDesk.disk.levelsInDesk.ToString();
        }

        if(currentDesk != null)
        {

            if (isLoading)
            {
                turnOffGameUI();
            }
            else if(!dialogue.dialogueOn)
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
                if (currentDesk.disk.firstInsertion)
                {
                    AudioManager.instance.playSound("mini game background music ");
                    AudioManager.instance.playSound("timer sound");
                    StartCoroutine(timerGiltching());
                    countTime = true;
                    currentDesk.disk.firstInsertion = false;
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
        
        
        if(currentDesk != null && timerUI.activeSelf && countTime && plugedIn)
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
                inAlertMode = true;
                StartCoroutine(triggerAlert());

            }
        }
      
        if (!plugedIn)
        {
            
            if(currentDesk != null && alertMode)
            {
                AudioManager.instance.stopSound("alert sound");
                playSound = true;
                exitComputerMode(0f);               
                Enemy.instanse.exexute = 0;
                StopAllCoroutines();
                
            }
            
        }

    }

    public void advanceLevel()
    {

        //for controlling the ui
        isLoading = true;
        countTime = false;
        
        levelCounter++;
        if(levelCounter < currentDesk.disk.levelsInDesk)
        {
            LevelManager.instance.destroyCurrentLevel();
            scoreUI.SetActive(false);
            timerUI.SetActive(false);
            tvloadingScreen.SetActive(true);
            Invoke("spawnLevel", TimeToLoadTheNextLevel);
        }      
        
    }
    

    //destroy the level that you were playing when you exit 
    void closeMiniGameLevel()
    {    
        levelCounter = 0;

        Object[] t = GameObject.FindObjectsOfType(typeof(LevelManager));
        if(t.Length > 0)
        {
            LevelManager.instance.destroyCurrentLevel();
        }   
    }

    
    //this code is hard coded , adjust it later 
    public void openComputer()
    {
        FloppyDisk d;
        //to check wether the player is holding in item or not 
        if (GameObject.Find("item holder").transform.childCount == 0)
        {
            Debug.Log("no item in hand ? ");
            return;
        }
        else { 
            floppyDiskInHand = GameObject.Find("item holder").transform.GetChild(0).gameObject;
            d = floppyDiskInHand.GetComponent<FloppyDisk>();
        }
     

        // to check if the item that the player is holding is a floppy desk 
        
        
        if (d != null && !d.disk.isFinished)
        {
            
            //to get the floopy desk information;
            if(currentDesk == null)
            {
                currentDesk = d;
            }

            thereIsFloopyDesk = true;

            if (currentDesk != null && !alertMode)
            {
                timeToFinichTheGame = currentDesk.disk.timeToFinichTheGame;
                //to put the player in computer mode
                computerScreen.GetComponent<MeshRenderer>().material = crtMat;
                AudioManager.instance.playSound("computer sound");        
                enterComputerMode();
                showBootupScreen();
                //this if statement id for showing the first dialogue only once, the first time he puts the disk in the computer 

            }
            else
            {
                Debug.Log("cant find the floopy disk");
                return;
            }

        }
        
    }
    
 
    
    //this is hard coded 
    void closeloading()
    {
        StopAllCoroutines();
        countTime = false;
        computerView.SetActive(false);
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);      
        MiniGameCam.SetActive(false);
        isUsingComputer = false;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        AudioManager.instance.stopSound("computer sound");
        screen.Release();
    }

    
    
    // adjust the computer mode from here
    void enterComputerMode()
    {
        titleScreen.SetActive(false);
        isLoading = true;
        countTime = false;
        timer = timeToFinichTheGame;
        isUsingComputer = true;
        floppyDiskInHand.transform.parent.gameObject.SetActive(false);
        MiniGameCam.SetActive(true);
        computerView.SetActive(true);
        tvLight.SetActive(true);
        tvLight.GetComponent<Light>().color = Color.white;
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        Invoke("showTitleScreen", (float)bootUpScreen[1].GetComponent<VideoPlayer>().length);
    }

    public void spawnLevel()
    {
        Instantiate(currentDesk.disk.levelManager, gameObject.transform).GetComponent<LevelManager>();
        
        
        //for controlling the ui       
        countTime = false;
        isLoading = false;      

        tvloadingScreen.SetActive(false);

        tvLight.GetComponent<Light>().color = currentDesk.disk.screenLightColor;


        if (currentDesk.disk.firstInsertion)
        {
            dialogue.startDialogue(currentDesk.disk.firstDialogue);       
        }
        else
        {
            AudioManager.instance.playSound("mini game background music ");
            countTime = true;
            AudioManager.instance.playSound("timer sound");
            StartCoroutine(timerGiltching());
        }

    }

    void showBootupScreen()
    {
        StartCoroutine(startboot());
    }

    IEnumerator startboot()
    {     
        bootUpScreen[0].gameObject.SetActive(true);
        bootUpScreen[1].GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds((float)bootUpScreen[1].GetComponent<VideoPlayer>().length);
        bootUpScreen[0].gameObject.SetActive(false);
    }

    void exitComputerMode(float time)
    {
        turnOffGameUI();
        inAlertMode = false;
        alertMode = false;
        thereIsFloopyDesk = false;    
        floppyDiskInHand.transform.parent.gameObject.SetActive(true);
        Invoke("closeloading", 0.3f);
        Invoke("closeMiniGameLevel", time);
    }
    
    
    void takeOutCurrentFloppyDisk()
    {
        if (playDialogue)
        {
            StopAllCoroutines();
            timerGiltchingFreq = 1000;
            AudioManager.instance.stopSound("timer sound");
            AudioManager.instance.stopSound("mini game background music ");
            dialogue.startDialogue(currentDesk.disk.LastDialogue);
            playDialogue = false;
        }

        Debug.Log("i should be loobing");
        if (!dialogue.dialogueOn)
        {
            a = false;
            currentDesk.disk.isFinished = true;
            playDialogue = true;
            levelCounter = 0;
            currentDesk = null;          
            exitComputerMode(1f);

        }     
    }

    void turnOffGameUI()
    {
        
        scoreUI.SetActive(false);
        timerUI.SetActive(false);
    }
    void turnOnGameUI()
    {
        scoreUI.transform.parent.gameObject.SetActive(true);
        scoreUI.SetActive(true);
        timerUI.SetActive(true);
    }

    void showTitleScreen()
    {

        tvLight.GetComponent<Light>().color = currentDesk.disk.gameTitleScreenColor;
        startgame = false;
        StartCoroutine(titleAnim());
        titleScreen.SetActive(true);
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentDesk.disk.gameTitle;
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = currentDesk.disk.gameTitleScreenColor;
    }

    IEnumerator titleAnim()
    {
        while (true)
        {
            GameObject t = titleScreen.transform.Find("Text (TMP)").gameObject;
            t.SetActive(!t.activeSelf);
            yield return new WaitForSeconds(0.35f);
        }
    }

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

    IEnumerator triggerAlert()
    {
        turnOffGameUI();
        GameObject T = GameObject.Find("2dplayer(Clone)");
        if(T != null)
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
        alertMode = true;
        computerView.SetActive(false);
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        floppyDiskInHand.transform.parent.gameObject.SetActive(true);
        isUsingComputer = false;
        Debug.Log("you falied beebb beeeb beeeb");
    }

    void timerSound(float time)
    {
        
        if(time > 30)
        {
            timerGiltchingFreq = 20f;
            AudioManager.instance.changePitch("timer sound", 0.65f);
        }
        else if(time < 30 && time > 20)
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


    public void turnOffComputer()
    {
        StopAllCoroutines();
        timerGiltchingFreq = 1000;
        AudioManager.instance.stopSound("mini game background music ");
        AudioManager.instance.stopSound("timer sound");
        exitComputerMode(1f);
    }
}
