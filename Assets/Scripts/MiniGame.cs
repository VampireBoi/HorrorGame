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


    // the desk inside the computer;
    [HideInInspector]
    public FloppyDisk currentDesk;

    //to check 
    public bool thereIsFloopyDesk;
    
    public RenderTexture screen;

    public Sprite loading;
    public Sprite closing;

    public GameObject text;

    [HideInInspector]public bool isUsingComputer = false;


    //the light coming from the screen when the game is running "change later but works for now" 
    public GameObject tvLight;
    public GameObject tvloadingScreen;

    private void Start()
    {   
        computerView.SetActive(false);
        MiniGameCam.SetActive(false);
        thereIsFloopyDesk = false;
        instance = this;
        tvLight.SetActive(false);
        tvloadingScreen.SetActive(false);
        ui.transform.FindChild("score").gameObject.SetActive(false);
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
            exitComputerMode();
        }  

        //finishing the desk in take it out of the system;
        if(thereIsFloopyDesk && levelCounter >= currentDesk.disk.levelsInDesk)
        {
            takeOutCurrentFloppyDisk();
        } 

        if(currentDesk != null && ui.transform.FindChild("score").gameObject.activeSelf)
        {
            ui.transform.FindChild("score").GetComponent<TextMeshProUGUI>().text = "level: " + (levelCounter + 1).ToString() + " out of: " + currentDesk.disk.levelsInDesk.ToString();
        }




    }

    public void advanceLevel()
    {
        levelCounter++;
        if(levelCounter < currentDesk.disk.levelsInDesk)
        {
            LevelManager.instance.destroyCurrentLevel();
            ui.transform.FindChild("score").gameObject.SetActive(false);    
            Invoke("spawnLevel", TimeToLoadTheNextLevel);
        }      
        
    }
    

    //destroy the level that you were playing when you exit 
    void closeMiniGameLevel()
    {
        if (isUsingComputer)
        {
            levelCounter = 0;
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

            if (currentDesk != null)
            {
                //to put the player in computer mode
                enterComputerMode();
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
        FirstPersonController.Instance.cameraCanMove = true;
        isUsingComputer = false;
        FirstPersonController.Instance.playerCanMove = true;
        screen.Release();
    }

    
    
    // adjust the computer mode from here
    void enterComputerMode()
    {
        isUsingComputer = true;
        floppyDiskInHand.transform.parent.gameObject.SetActive(false);
        MiniGameCam.SetActive(true);
        computerView.SetActive(true);
        tvLight.SetActive(true);
        tvloadingScreen.SetActive(true);
        text.GetComponent<Text>().text = currentDesk.disk.startScene;
        tvloadingScreen.transform.FindChild("text").GetComponent<SpriteRenderer>().sprite = loading;
        FirstPersonController.Instance.playerCanMove = false;
        FirstPersonController.Instance.cameraCanMove = false;
        Invoke("spawnLevel", 2f);
    }

    public void spawnLevel()
    {
        Instantiate(miniGameManager, gameObject.transform).GetComponent<LevelManager>();
  
        ui.transform.FindChild("score").gameObject.SetActive(true);

    }

    void exitComputerMode()
    {
        thereIsFloopyDesk = false;
        closeMiniGameLevel();
        tvloadingScreen.transform.FindChild("text").GetComponent<SpriteRenderer>().sprite = closing;
        floppyDiskInHand.transform.parent.gameObject.SetActive(true);
        Invoke("closeloading", 1f);
    }
    
    
    void takeOutCurrentFloppyDisk()
    {
        text.GetComponent<Text>().text = currentDesk.disk.exitScene;
        currentDesk.disk.isFinished = true;
        thereIsFloopyDesk = false;
        currentDesk = null;
        levelCounter = 0;
        exitComputerMode();
    }

    

    

}
