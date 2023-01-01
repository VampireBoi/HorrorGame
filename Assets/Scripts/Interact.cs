using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public Dialogue dialogue;

    public static Interact Instance;
    public Transform chairPosition;
    public LayerMask mask;
    public Transform exitPosition;
    GameObject[] player;
    public static bool isSetting = false;
    public GameObject fpsUI;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f, mask.value) && !isSetting && !uiManager.instance.inventoryIsOpen && !Computer.instance.isUsingComputer)
        {

            // there is an interactable object the game shows the hand icon 

            //FirstPersonController.Instance.crosshairObject.gameObject.SetActive(true);

            
            if(hit.transform.tag == "generatorPlace" && ItemHolder.instance.transform.GetChild(0).name == "generator item(Clone)")
            {
                GeneratorPlace.instance.showHighLight = true;
                if(Input.GetKeyDown(KeyCode.Mouse0) && !isSetting)
                {
                    GeneratorPlace.instance.spownGenerator();
                }
            }
            else
            {
                //Debug.Log("dsdsdsd");
                GeneratorPlace.instance.showHighLight = false;
            }




            if (!TheFirstPerson.FPSController.instance.movementEnabled)
            {
                fpsUI.transform.GetChild(0).gameObject.SetActive(false);
                fpsUI.transform.GetChild(1).gameObject.SetActive(false);
            }

            else if(hit.transform.tag == "wall")
            {
                fpsUI.transform.GetChild(0).gameObject.SetActive(true);
                fpsUI.transform.GetChild(1).gameObject.SetActive(false);
            } else
            {
                fpsUI.transform.GetChild(0).gameObject.SetActive(false);
                fpsUI.transform.GetChild(1).gameObject.SetActive(true);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isSetting)
            {
                // if the player is looking at hte save area "the bed"
                if (hit.transform.tag == "save area")
                {

                    //AudioManager.instance.stopSound("player footsteps");
                    enterSaveArea();
                }

                // if the player is looking at an item
                if (hit.transform.tag == "item")
                {
                   if(hit.transform.name == "generator item(Clone)")
                    {
                        if (GeneratorPlace.instance.thereIsGenerator)
                        {
                            GeneratorPlace.instance.thereIsGenerator = false;
                        }
                    }
                    hit.transform.GetComponent<ItemPickup>().pickup();
                }
                if (hit.transform.tag == "computer" && !Computer.instance.alertMode)
                {
                    
                    if(GameManager.Instance.level == 3)
                    {
                        if (GeneratorPlace.instance.thereIsGenerator)
                        {
                            if (GeneratorPlace.instance.generatorInstance.power > 0.05f)
                            {
                                Computer.instance.openComputer();
                            }
                            else { Debug.Log("there is no power in the generator"); }
                        }
                        else
                        {
                            Debug.Log("there is no power");
                        }
                        
                    }
                    else
                    {
                        if (Computer.instance.plugedIn)
                        {
                            Computer.instance.openComputer();
                        }
                        else
                        {
                            Debug.Log("computer not pluged in");
                        }
                    }
                    
                                  
                }

                if (hit.transform.tag == "Plug")
                {
                    //Debug.Log("plug the computer");
                    if (Computer.instance.plugedIn)
                    {
                        AudioManager.instance.playSound("power on");
                    }
                    else
                    {
                        AudioManager.instance.playSound("power off");
                    }
                    Computer.instance.plugedIn = !Computer.instance.plugedIn;
                }

                if(hit.transform.tag == "KeyPad")
                {
                    if (!KeyPadPuzzle.instance.inKeyPadMode)
                    {
                        KeyPadPuzzle.instance.enterkeyPadMode();

                    }
                }

                if(hit.transform.tag == "Door")
                {
                    if (!KeyPadPuzzle.instance.inKeyPadMode)
                    {
                        hit.transform.GetComponent<Door>().tryOpenTheDoor();
                    }
                }

                if(hit.transform.name == "cranck")
                {
                    hit.transform.GetComponent<GeneratorCranck>().AddPowerToGenerator();
                }

            }


        }
        // there's no intaractable object ahed, deactivate the hand icon 
        else {

         
            //Debug.Log("dsdsdsd");
            GeneratorPlace.instance.showHighLight = false;
            


            if (Computer.instance.isUsingComputer)
            {
                fpsUI.transform.GetChild(0).gameObject.SetActive(false);
                fpsUI.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                fpsUI.transform.GetChild(0).gameObject.SetActive(true);
                fpsUI.transform.GetChild(1).gameObject.SetActive(false);
            }
            
        }//FirstPersonController.Instance.crosshairObject.gameObject.SetActive(false); }
        
        
        
       // to get out of bed 
        if (isSetting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse2) && !dialogue.dialogueOn)
            {
                StartCoroutine(ExitSave());
            }

        }
    }

    
    public void enterSaveArea()
    {
        StartCoroutine(EnterSave());
    }
    IEnumerator EnterSave()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

        player[0].transform.position = chairPosition.position;
        TheFirstPerson.FPSController.instance.crouching = true;
        Invoke("settDown", (TheFirstPerson.FPSController.instance.crouchTransitionSpeed * TheFirstPerson.FPSController.instance.crouchColliderHeight) / 50f);
        isSetting = true;
        ItemHolder.instance.destroyItemInHand();
    }
    void settDown()
    {

        player[0].transform.position = chairPosition.position;
        TheFirstPerson.FPSController.instance.movementEnabled = false;
    }


    IEnumerator ExitSave()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

        player[0].transform.position = exitPosition.position;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        TheFirstPerson.FPSController.instance.crouching = false;

        ItemHolder.instance.holdFirstItem();
        isSetting = false;

    }
}
