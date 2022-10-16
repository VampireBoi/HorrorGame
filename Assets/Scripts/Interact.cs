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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,5f, mask) && !isSetting && !uiManager.instance.inventoryIsOpen && !MiniGame.instance.isPlaying){
           
            // there is an interactable object the game shows the hand icon 
            FirstPersonController.Instance.crosshairObject.gameObject.SetActive(true);                                
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isSetting)
            {
                // if the player is looking at hte save area "the bed"
                if (hit.transform.tag == "save area")
                {

                    AudioManager.instance.stopSound("player footsteps");
                    player[0].transform.position = chairPosition.position;
                    player[0].GetComponent<FirstPersonController>().playerCanMove = false;
                    player[0].GetComponent<FirstPersonController>().enableJump = false;
                    player[0].GetComponent<FirstPersonController>().enableCrouch = false;
                    player[0].GetComponent<FirstPersonController>().enableHeadBob = false;
                    isSetting = true;
                    itemSway.instance.destroyItemInHand();
                }
                
                // if the player is looking at an item
                if (hit.transform.tag == "item")
                {
                    hit.transform.GetComponent<ItemPickup>().pickup();         
                }
                if(hit.transform.tag == "computer")
                {
                    Debug.Log("sddssdsdsdsdsdsdsd");
                    MiniGame.instance.playMiniGame();
                }
            }
            
            
        }
        // there's no intaractable object ahed, deactivate the hand icon 
        else { FirstPersonController.Instance.crosshairObject.gameObject.SetActive(false); }
        
        
        
       // to get out of bed 
        if (isSetting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                player[0].GetComponent<FirstPersonController>().playerCanMove = true;
                player[0].GetComponent<FirstPersonController>().enableJump = true;
                player[0].GetComponent<FirstPersonController>().enableCrouch = true;
                player[0].GetComponent<FirstPersonController>().enableHeadBob = true;
                player[0].transform.position = exitPosition.position;
                itemSway.instance.holdFirstItem();
                isSetting = false;
            }

        }
    }
}
