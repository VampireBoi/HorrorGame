using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThirdMiniGame : MonoBehaviour
{
    public static ThirdMiniGame instance;

    public GameObject inviroment;
    public GameObject miniGamePlayer;
    public GameObject enemy;
    GameObject i;
    GameObject m;
    GameObject e;


    [HideInInspector] public bool gameIsFinished;


    [Header("difficulty values")]
    // for the game -------------------------------------------
    public float timeToChick;
    public float checkingTime;
    public float coolDownTime;
    public float timeToFinishTheGame;


    [HideInInspector] public bool gameIsActive;

    // for the computer-----------------------------------------


    public int levelCounter;
    public int levelsToWin;

     
    
    float timer;

    //public GameObject titleScreen;
    Material crtMat;
    Material GiltchMat;
    GameObject computerScreen;
    float timerGiltchingFreq;
    bool startgame;
    GameObject MiniGameCam;
    Dialogue dialogue;
    bool playDialogue;

    GameObject timeUI;
    GameObject scoreUI;
    GameObject timerUI;

    bool a;

    bool wintheGame = false;



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
        timer = timeToFinishTheGame;

        timeUI = Computer.instance.ui.transform.Find("third game ui").gameObject;
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

        spawnLevel();
        MiniGameCam.transform.position = new Vector3(i.transform.position.x, i.transform.position.y - 1f, i.transform.position.z - 0.9f);


    }

    // Update is called once per frame
    void Update()
    {


        if (a == true && !dialogue.dialogueOn)
        {

            Debug.Log("a dialouge has been ended");
            //turnOnGameUI();
            AudioManager.instance.playSound("third game music");

            GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
            if (p != null)
            {
                p.GetComponent<ThirdMiniGamePlayer>().canMove = true;
            }


            if (Computer.instance.currentDesk.disk.firstInsertion)
            {
                //AudioManager.instance.playSound("mini game background music ");
                //AudioManager.instance.playSound("timer sound");
                //StartCoroutine(timerGiltching());
                //countTime = true;
                Computer.instance.currentDesk.disk.firstInsertion = false;
            }

            turnOnGameUI();
            gameIsActive = true;
            a = false;
        }
        if (dialogue.dialogueOn && a == false)
        {
            Debug.Log("a dialouge has been started");
            AudioManager.instance.stopSound("third game music");


            GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
            if (p != null)
            {
                p.GetComponent<ThirdMiniGamePlayer>().canMove = false;
            }

            turnOffGameUI();
            gameIsActive = false;
            a = true;
        }

    
        
        if (gameIsActive)
        {
            if(wintheGame)
            {
                winTheGame();
            }
            if (timer <= 0)
            {
                GameOver();
            }
            else { timer -= Time.deltaTime; }

            if (timeUI.activeSelf)
            {
                timeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time: " + (int)timer;
                timeUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "level: " + levelCounter;
            }
        }
    }







    public void spawnLevel()
    {
        i = Instantiate(inviroment);
        m = Instantiate(miniGamePlayer);
        e = Instantiate(enemy);

        //for controlling the ui       
        countTime = false;
        isLoading = false;

        tvloadingScreen.SetActive(false);

        //tvLight.GetComponent<Light>().color = Computer.instance.currentDesk.disk.screenLightColor;

        if (Computer.instance.currentDesk.disk.firstInsertion)
        {
            Invoke("startFirstDialouge", 2f);
            m.GetComponent<ThirdMiniGamePlayer>().canMove = false;

        }
        else
        {
            //StartCoroutine(timerGiltching());  
            m.GetComponent<ThirdMiniGamePlayer>().canMove=true;
            gameIsActive = true;

        }
        //AudioManager.instance.playSound("second game background music");
        //AudioManager.instance.changePitch("second game background music", 1f);
        AudioManager.instance.playSound("third game music");

        Invoke("turnOnGameUI", 0.3f);

    }


    IEnumerator progressRound()
    {
        if (levelCounter < levelsToWin)
        {
            levelCounter++;
            timer += 10f;
            gameIsActive = false;
            increaseDifficulty(0.4f, 0.6f, 0.5f);
            turnOffGameUI();
            //AudioManager.instance.stopSound("second game background music");
            Destroy(i);
            Destroy(m);
            Destroy(e);
            yield return new WaitForSeconds(1f);


            spawnLevel();
            gameIsActive = true;
        }
        else { wintheGame = true; }
       
        

    }

    public void progress()
    {
        StartCoroutine(progressRound());

    }




    void turnOffGameUI()
    {
        scoreUI.SetActive(false);
        timerUI.SetActive(false);
        timeUI.SetActive(false);
    }
    void turnOnGameUI()
    {
        timeUI.SetActive(true);
    }

    void startFirstDialouge()
    {
        turnOffGameUI();
        AudioManager.instance.stopSound("mini game background music ");
        dialogue.startDialogue(Computer.instance.currentDesk.disk.firstDialogue);
    }



    public void closeMiniGame()
    {
        StopAllCoroutines();
        turnOffGameUI();
        AudioManager.instance.stopSound("third game music");
        Destroy(i);
        Destroy(m);
        Destroy(e);
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
            AudioManager.instance.stopSound("third game music");
            dialogue.startDialogue(Computer.instance.currentDesk.disk.LastDialogue);
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
        GameObject p = GameObject.FindGameObjectWithTag("2dPlayer");
        if (p != null)
        {
            p.GetComponent<ThirdMiniGamePlayer>().canMove = false;
        }
        Computer.instance.inAlertMode = true;
        turnOffGameUI();
        gameIsActive = false;
        AudioManager.instance.stopSound("third game music");


        
        yield return new WaitForSeconds(1f);

        AudioManager.instance.playSound("alert sound");


        computerScreen.GetComponent<MeshRenderer>().material = GiltchMat;

        yield return new WaitForSeconds(0.8f);

        Computer.instance.alertMode = true;

        TheFirstPerson.FPSController.instance.movementEnabled = true;

        Debug.Log("you falied beebb beeeb beeeb");
    }



    public void increaseDifficulty(float decreaseCoolDown, float decreaseTimeToChick, float increaseChickingTime)
    {
        coolDownTime -= decreaseCoolDown;
        timeToChick -= decreaseTimeToChick;
        checkingTime -= increaseChickingTime;
    }

}
