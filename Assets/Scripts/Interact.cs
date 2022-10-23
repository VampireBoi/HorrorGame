using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f, mask.value) && !isSetting && !uiManager.instance.inventoryIsOpen && !MiniGame.instance.isUsingComputer)
        {

            // there is an interactable object the game shows the hand icon 
            //FirstPersonController.Instance.crosshairObject.gameObject.SetActive(true);
            if(hit.transform.tag == "wall")
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
                    player[0].transform.position = chairPosition.position;                 
                    TheFirstPerson.FPSController.instance.crouching = true;
                    Invoke("settDown", (TheFirstPerson.FPSController.instance.crouchTransitionSpeed * TheFirstPerson.FPSController.instance.crouchColliderHeight) / 50f);
                    isSetting = true;
                    itemSway.instance.destroyItemInHand();
                }

                // if the player is looking at an item
                if (hit.transform.tag == "item")
                {
                    hit.transform.GetComponent<ItemPickup>().pickup();
                }
                if (hit.transform.tag == "computer")
                {
                    if (MiniGame.instance.plugedIn)
                    {
                        MiniGame.instance.openComputer();
                    }
                    else
                    {
                        Debug.Log("computer not pluged in");
                    }
                    
                }

                if (hit.transform.tag == "Plug")
                {
                    Debug.Log("plug the computer");
                    MiniGame.instance.plugedIn = !MiniGame.instance.plugedIn;
                }
            }


        }
        // there's no intaractable object ahed, deactivate the hand icon 
        else {
            fpsUI.transform.GetChild(0).gameObject.SetActive(true);
            fpsUI.transform.GetChild(1).gameObject.SetActive(false);
        }//FirstPersonController.Instance.crosshairObject.gameObject.SetActive(false); }
        
        
        
       // to get out of bed 
        if (isSetting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                player[0].transform.position = exitPosition.position;
                TheFirstPerson.FPSController.instance.movementEnabled = true;
                TheFirstPerson.FPSController.instance.crouching = false;
         
                itemSway.instance.holdFirstItem();
                isSetting = false;
            }

        }
    }

    void settDown()
    {

        player[0].transform.position = chairPosition.position;
        TheFirstPerson.FPSController.instance.movementEnabled = false;
    }
}
