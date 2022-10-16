using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public static MiniGame instance;
    
    [Header("the time to load the next level \"in secends\"")]
    public float TimeToLoadTheNextLevel;

    public GameObject miniGameManager;

    public GameObject floppyDisk; 
    public int level;

    public RenderTexture screen;

    public Sprite loading;
    public Sprite closing;

    [HideInInspector]
    public float difficulty;
    [HideInInspector]public bool isPlaying = false;


    //the light coming from the screen when the game is running "change later but works for now" 
    public GameObject tvLight;
    public GameObject tvloadingScreen;


    private void Start()
    { 
        instance = this;
        tvLight.SetActive(false);
        tvloadingScreen.SetActive(false);       
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closeMiniGameLevel();
            closingComputerScreen();
        }      
    }

    public void advanceLevel()
    {
        level++;
        LevelManager.instance.destroyCurrentLevel();
        // here we put the animation between the levels
        Invoke("spawnLevel", TimeToLoadTheNextLevel);
    }

    public void playMiniGame()
    {
        if(GameObject.Find("item holder").transform.childCount == 0)
        {
            return;
        }
        else { floppyDisk = GameObject.Find("item holder").transform.GetChild(0).gameObject; }

        openingComputerScreen();
        
        
    }

    public void startMiniGameLevel()
    {
        FloppyDisk pd;
        floppyDisk.TryGetComponent<FloppyDisk>(out pd);
        if (pd != null)
        {
            isPlaying = true;
            difficulty = pd.difficulty;
            LevelManager m = spawnLevel();
            Debug.Log("difficulty is " + pd.difficulty);
            FirstPersonController.Instance.playerCanMove = false;                    
        }
    }



    void closeMiniGameLevel()
    {
        if (isPlaying)
        {
            level = 0;
            LevelManager.instance.destroyCurrentLevel();
            FirstPersonController.Instance.playerCanMove = true;
            isPlaying = false;
        }
    }

    
    //this code is hard coded , adjust it later 
    void openingComputerScreen()
    {

        FloppyDisk pd;
        floppyDisk.TryGetComponent<FloppyDisk>(out pd);
        if (pd != null)
        {
            floppyDisk.SetActive(false);
            tvLight.SetActive(true);
            tvloadingScreen.SetActive(true);
            tvloadingScreen.transform.FindChild("text").GetComponent<SpriteRenderer>().sprite = loading;
            Invoke("startMiniGameLevel", 1f);
        }
        
    }
    //this code is hard coded , adjust it later
    void closingComputerScreen()
    {
        tvloadingScreen.transform.FindChild("text").GetComponent<SpriteRenderer>().sprite = closing;
        GameObject.Find("item holder").SetActive(true);
        GameObject.Find("item holder").transform.GetChild(0).gameObject.SetActive(true);
        Invoke("closeloading", 1f);
    }

    void closeloading()
    {
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);
        screen.Release();
    }


    public LevelManager spawnLevel()
    {
        return Instantiate(miniGameManager, gameObject.transform).GetComponent<LevelManager>();
    }

}
