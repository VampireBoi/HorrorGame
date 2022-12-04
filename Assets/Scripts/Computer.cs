using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
public class Computer : MonoBehaviour
{
    public static Computer instance;

    public GameObject ui;

    [HideInInspector]
    public GameObject floppyDiskInHand;
    [HideInInspector]
    public ItemData itemInHand;

    [HideInInspector]
    public GameObject itemHolder;


    public GameObject titleScreen;

    public Material crtMat;
    public Material GiltchMat;

    float timerGiltchingFreq;

    public GameObject computerScreen;

    bool startgame;
   
    public GameObject MiniGameCam;

    public GameObject computerView;

    public Dialogue dialogue;
    // the desk inside the computer;
    [HideInInspector]
    public FloppyDeskData currentDesk;

    public GameObject currentGame;

    //to check 
    public bool thereIsFloopyDesk;

    public RenderTexture screen;

    bool playDialogue;

    [HideInInspector] public bool computerOn;

    public GameObject text;

    [HideInInspector] public bool isUsingComputer = false;

    public GameObject scoreUI;
    public GameObject timerUI;

    public GameObject[] bootUpScreen;


    float timeToFinichTheGame;

    public bool plugedIn = false;


    bool isLoading = false;

    bool playSound;

    //when the timer runs out and the computer makes the beb sound
    [HideInInspector] public bool alertMode;
    [HideInInspector] public bool inAlertMode;


    //the light coming from the screen when the game is running "change later but works for now" 
    public GameObject tvLight;
    public GameObject tvloadingScreen;

    private void Start()
    {
        alertMode = false;
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
        //Debug.Log("is using the coumputer: " + isUsingComputer);
        //Debug.Log("in alert mode: " + inAlertMode);

        
        if (Input.GetKeyDown(KeyCode.G) && isUsingComputer && !inAlertMode && !dialogue.dialogueOn)
        {
            leaveComputer();
        }


        if (Input.GetKeyDown(KeyCode.R) && isUsingComputer && !inAlertMode && !dialogue.dialogueOn)
        {
            reloadTheComputer();
            int r = Random.Range(0, 2);
            Debug.Log(r);
            if(r == 1)
            {
                Enemy.instanse.gocheckTheRoom();
            }
        }

        // here we chick if the game that we're currenlty playing is finished 
        if (currentGame != null)
        {
            // here we chick for each game 

            //insted of getting the component let's chick for the instance value
            if(FirstMiniGame.instance != null)
            {
                if (FirstMiniGame.instance.gameIsFinished && currentDesk != null)
                {
                    takeOutCurrentFloppyDisk();
                }
            } else if(SecondMiniGame.instance != null)
            {
                if (SecondMiniGame.instance.gameIsFinished && currentDesk != null)
                {
                    takeOutCurrentFloppyDisk();
                }
            }
            else if (ThirdMiniGame.instance != null)
            {
                if (ThirdMiniGame.instance.gameIsFinished && currentDesk != null)
                {
                    takeOutCurrentFloppyDisk();
                }
            }


            //if(currentGame.GetComponent<secondMiniGame>().gameIsFinished) ....
        }

        if (!startgame && Input.GetKeyDown(KeyCode.Return) && isUsingComputer && titleScreen.activeSelf)
        {
            startgame = true;
        }

        if (startgame)
        {
            timerGiltchingFreq = 500;
            StopAllCoroutines();
            titleScreen.SetActive(false);
            Invoke("spawnGame", 0.2f);
            startgame = false;
        }


        if (!plugedIn)
        {           
            if (alertMode)
            {
            
                AudioManager.instance.stopSound("alert sound");
                inAlertMode = false;        
                playSound = true;
                alertMode = false;
                

            }
            if (computerOn)
            {
                exitComputerMode(0f);
                Enemy.instanse.exexute = 0;
                StopAllCoroutines();
                computerOn = false;
            }
            

        }
        else
        {
            if (alertMode)
            {
                alertMode = true;
                computerView.SetActive(false);
                TheFirstPerson.FPSController.instance.movementEnabled = true;
                itemHolder.gameObject.SetActive(true);
                isUsingComputer = false;
            }
        }

        

    }
 

    //this code is hard coded , adjust it later 
    public void openComputer()
    {

        StartCoroutine(Open());
    }
    IEnumerator Open()
    {
        if (!computerOn)
        {
            FloppyDeskData d;
            //to check wether the player is holding in item or not 
            if (GameObject.Find("item holder").transform.childCount == 0)
            {
                Debug.Log("no item in hand ? ");
                yield break;
            }
            else
            {
                itemHolder = GameObject.Find("item holder").gameObject;
                floppyDiskInHand = itemHolder.transform.GetChild(0).gameObject;

                d = floppyDiskInHand.GetComponent<FloppyDisk>().disk;

            }


            // to check if the item that the player is holding is a floppy desk 


            if (d != null && !d.isFinished)
            {

                //to get the floopy desk information;
                if (currentDesk == null)
                {
                    currentDesk = d;

                    // for the first puzzle
                    if (GameManager.Instance.level == 1)
                    {
                        currentDesk.LastDialogue[currentDesk.LastDialogue.Length - 1] = KeyPadPuzzle.instance.password;
                    }
                    else { currentDesk.LastDialogue[currentDesk.LastDialogue.Length - 1] = ""; }
                }

                thereIsFloopyDesk = true;

                if (currentDesk != null && !alertMode)
                {
                    if (currentDesk.firstInsertion)
                    {
                        Enemy.instanse.canChick = false;
                    }
                    
                    fadeAnim.instance.startFadeAnim();
                    yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

                    //to put the player in computer mode
                    itemInHand = floppyDiskInHand.gameObject.GetComponent<ItemPickup>().item;
                    InventoryManager.Instance.removeItem(itemInHand);
                    ItemHolder.instance.destroyItemInHand();

                    computerScreen.GetComponent<MeshRenderer>().material = crtMat;
                    AudioManager.instance.playSound("computer sound");
                    enterComputerMode();

                    //this if statement id for showing the first dialogue only once, the first time he puts the disk in the computer 

                }
                else
                {
                    Debug.Log("cant find the floopy disk");
                    yield break;
                }

            }
        }
        else {
            fadeAnim.instance.startFadeAnim();
            yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

            enterComputerMode(); 
        }

    }


    //this is hard coded 
    void closeloading()
    {       
        StopAllCoroutines();      
        computerView.SetActive(false);
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);
        MiniGameCam.SetActive(false);
        isUsingComputer = false;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        AudioManager.instance.stopSound("computer sound");
        screen.Release();
        if (!itemInHand.prefab.GetComponent<FloppyDisk>().disk.isFinished)
        {
            InventoryManager.Instance.addItem(itemInHand);
        }    
        itemHolder.gameObject.SetActive(true);
        currentDesk = null;
        computerOn = false;
        
    }

    void closeloading2()
    {
        StopAllCoroutines();
        //computerView.SetActive(false);
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);
        MiniGameCam.SetActive(false);
        //isUsingComputer = false;
        //TheFirstPerson.FPSController.instance.movementEnabled = true;
        AudioManager.instance.stopSound("computer sound");
        screen.Release();
        computerOn = false;
        Invoke("restart", 0.5f);
    }

    void restart()
    {
        computerScreen.GetComponent<MeshRenderer>().material = crtMat;
        AudioManager.instance.playSound("computer sound");
        enterComputerMode();
    }



    // adjust the computer mode from here
    void enterComputerMode()
    {

        isUsingComputer = true;
        isLoading = true;             
        MiniGameCam.SetActive(true);
        computerView.SetActive(true); 
        tvLight.SetActive(true);          
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        itemHolder.gameObject.SetActive(false);

        if (!computerOn)
        {
            thereIsFloopyDesk = true;
            titleScreen.SetActive(false);
            showBootupScreen();
            computerOn = true;

        }
   
        
    }

    public void spawnGame()
    {
        //Instantiate(currentDesk.disk.levelManager, gameObject.transform).GetComponent<LevelManager>();

        currentGame = Instantiate(currentDesk.miniGame, gameObject.transform);
        //miniGame.SetActive(true);
        //for controlling the ui       
        //countTime = false;
        //isLoading = false;

        //tvloadingScreen.SetActive(false);

        tvLight.GetComponent<Light>().color = currentDesk.screenLightColor;

        //if (currentDesk.disk.firstInsertion)
        //{
            //dialogue.startDialogue(currentDesk.disk.firstDialogue);
        //}
        //else
        //{
            //AudioManager.instance.playSound("mini game background music ");
            //countTime = true;
            //AudioManager.instance.playSound("timer sound");
            //StartCoroutine(timerGiltching());
        //}

    }

    void showBootupScreen()
    {
        StartCoroutine(startboot());
    }

 
    void exitComputerMode(float time)
    {    
        // to destroy a mini game if there's any 
        if(currentGame != null)
        {
            // here we chick for each game 
            if(FirstMiniGame.instance != null)
            {
                FirstMiniGame.instance.closeMiniGame();
            }
            else if(SecondMiniGame.instance != null)
            {
                SecondMiniGame.instance.closeMiniGame();
            }
            else if(ThirdMiniGame.instance != null)
            {
                ThirdMiniGame.instance.closeMiniGame();
            }

            //if(SecondMiniGame != null) ...
        }       
        turnOffGameUI();
        inAlertMode = false;
        alertMode = false;
        thereIsFloopyDesk = false;
        
        currentGame = null;
        //floppyDiskInHand.transform.parent.gameObject.SetActive(true);

        
        Invoke("closeloading", time);
        //Invoke("closeloading", 0.3f);    
    }



    void reloadComputer(float time)
    {
        // to destroy a mini game if there's any 
        if (currentGame != null)
        {
            // here we chick for each game 
            if (FirstMiniGame.instance != null)
            {
                FirstMiniGame.instance.closeMiniGame();
            }
            else if (SecondMiniGame.instance != null)
            {
                SecondMiniGame.instance.closeMiniGame();
            }
            else if (ThirdMiniGame.instance != null)
            {
                ThirdMiniGame.instance.closeMiniGame();

            }

            //if(SecondMiniGame != null) ...
        }
        turnOffGameUI();
        inAlertMode = false;
        alertMode = false;
        thereIsFloopyDesk = false;

        currentGame = null;       
        Invoke("closeloading2", 0.3f);
        //Invoke("closeloading", 0.3f);    
    }



    public void takeOutCurrentFloppyDisk()
    {
        if(GameManager.Instance.level == 2)
        {
            GameManager.Instance.level++;
        }
        Debug.Log(currentDesk);
        currentDesk.isFinished = true;
        currentDesk = null;
        //GameManager.Instance.level++;
        exitComputerMode(0.3f);
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

    void showTitleScreen()
    {
        tvLight.GetComponent<Light>().color = currentDesk.gameTitleScreenColor;
        startgame = false;
        StartCoroutine(titleAnim());
        titleScreen.SetActive(true);
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentDesk.gameTitle;
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = currentDesk.gameTitleScreenColor;
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
    IEnumerator startboot()
    {
        tvLight.GetComponent<Light>().color = Color.white;
        bootUpScreen[0].gameObject.SetActive(true);
        bootUpScreen[1].GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds((float)bootUpScreen[1].GetComponent<VideoPlayer>().length);
        bootUpScreen[0].gameObject.SetActive(false);
        showTitleScreen();
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
        yield break;
    }



    public void reloadTheComputer()
    {
        StopAllCoroutines();
        timerGiltchingFreq = 1000;
        AudioManager.instance.stopSound("mini game background music ");
        AudioManager.instance.stopSound("timer sound");
        reloadComputer(0.5f);
    }

    public void turnOffComputer()
    {
        StopAllCoroutines();
        timerGiltchingFreq = 1000;
        AudioManager.instance.stopSound("mini game background music ");
        AudioManager.instance.stopSound("timer sound");
        exitComputerMode(1f);
    }

    public void leaveComputer()
    {
        StartCoroutine(leave());
    }

    IEnumerator leave()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

        computerView.SetActive(false);
        isUsingComputer = false;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        itemHolder.gameObject.SetActive(true);
        //GameObject.Find("item holder").gameObject.SetActive(true);
    }


}
