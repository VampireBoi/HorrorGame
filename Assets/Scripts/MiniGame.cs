using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGame : MonoBehaviour
{
    public static MiniGame instance;
    
    [Header("the time to load the next level \"in secends\"")]
    public float TimeToLoadTheNextLevel;

    public GameObject ui;

    public GameObject miniGameManager;
  
    [HideInInspector]
    public GameObject floppyDiskInHand; 
    public int levelCounter;

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
        scoreUI = ui.transform.FindChild("score").gameObject;
        timerUI = ui.transform.FindChild("Timer").gameObject;
        playDialogue = true;
        computerView.SetActive(false);
        MiniGameCam.SetActive(false);
        thereIsFloopyDesk = false;
        instance = this;
        tvLight.SetActive(false);
        tvloadingScreen.SetActive(false);
        ui.transform.FindChild("score").gameObject.SetActive(false);

        playSound = true;
    }


    private void Update()
    {
        if (MiniGameCam.activeSelf && GameObject.FindGameObjectWithTag("2dPlayer") != null)
        {
            MiniGameCam.transform.position = GameObject.FindGameObjectWithTag("2dPlayer").transform.position;
            MiniGameCam.transform.position = new Vector3(MiniGameCam.transform.position.x, MiniGameCam.transform.position.y, -1);
        }   
        //to exit the computer mode
        if (Input.GetKeyDown(KeyCode.G) && isUsingComputer)
        {           
            exitComputerMode(1f);
        }  

        //finishing the desk in take it out of the system;
        if(thereIsFloopyDesk && levelCounter >= currentDesk.disk.levelsInDesk)
        {
            countTime = false;
            takeOutCurrentFloppyDisk();
        } 

        if(currentDesk != null && scoreUI.gameObject.activeSelf)
        {
            scoreUI.GetComponent<TextMeshProUGUI>().text = "level: " + (levelCounter + 1).ToString() + " out of: " + currentDesk.disk.levelsInDesk.ToString();
        }

        if(currentDesk != null && timerUI.activeSelf && countTime)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time:" + (int)timer + " s";
            }
            else
            {
                //here we trigger the timeout event
                if (playSound)
                {
                    AudioManager.instance.playSound("alert sound");
                    playSound = false;
                }
                alertMode = true;
                computerView.SetActive(false);
                TheFirstPerson.FPSController.instance.movementEnabled = true;
                floppyDiskInHand.transform.parent.gameObject.SetActive(true);
                isUsingComputer = false;
                Debug.Log("you falied beebb beeeb beeeb");               
            }
        }
        if (dialogue.dialogueOn || isLoading)
        {
            turnOffGameUI();
        }
        else
        {
            turnOnGamUI();
        }

        if (!plugedIn)
        {
            if(currentDesk != null && alertMode)
            {
                AudioManager.instance.stopSound("alert sound");
                playSound = true;
                exitComputerMode(0f);
                alertMode = false;
                Enemy.instanse.exexute = 0;
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
        LevelManager.instance.destroyCurrentLevel();        
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
                AudioManager.instance.playSound("computer sound");
                enterComputerMode();
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
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);
        computerView.SetActive(false);
        MiniGameCam.SetActive(false);
        isUsingComputer = false;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        AudioManager.instance.stopSound("computer sound");
        screen.Release();
    }

    
    
    // adjust the computer mode from here
    void enterComputerMode()
    {
        isLoading = true;
        countTime = false;
        timer = timeToFinichTheGame;
        isUsingComputer = true;
        floppyDiskInHand.transform.parent.gameObject.SetActive(false);
        MiniGameCam.SetActive(true);
        computerView.SetActive(true);
        tvLight.SetActive(true);      
        
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        Invoke("spawnLevel", 3f);
    }

    public void spawnLevel()
    {
        Instantiate(currentDesk.disk.levelManager, gameObject.transform).GetComponent<LevelManager>();
        
        //for controlling the ui 
        countTime = true;
        isLoading = false;

        tvloadingScreen.SetActive(false);

        if (currentDesk.disk.firstInsertion)
        {
            dialogue.startDialogue(currentDesk.disk.firstDialogue);
            currentDesk.disk.firstInsertion = false;
        }
    }

    void exitComputerMode(float time)
    {
        countTime = false;
        scoreUI.SetActive(false);
        timerUI.SetActive(false);
        thereIsFloopyDesk = false;
        closeMiniGameLevel();
        floppyDiskInHand.transform.parent.gameObject.SetActive(true);
        Invoke("closeloading", time);
    }
    
    
    void takeOutCurrentFloppyDisk()
    {

        if (playDialogue)
        {         
            dialogue.startDialogue(currentDesk.disk.LastDialogue);
            playDialogue = false;
        }

        Debug.Log("i should be loobing");
        if (!dialogue.dialogueOn)
        {
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
    void turnOnGamUI()
    {
        scoreUI.SetActive(true);
        timerUI.SetActive(true);
    }
    

    

}
