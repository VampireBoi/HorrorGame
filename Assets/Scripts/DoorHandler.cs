using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    bool isOpen;
    public Animator animator;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("enterd");
            if (!isOpen)
            {
                AudioManager.instance.playSound("open door");
                animator.SetBool("DoorIsOpen", true);
                Invoke("open", 1f);
            }
            //open door;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            Debug.Log("left");
            if (isOpen)
            {
                AudioManager.instance.playSound("close door");
                animator.SetBool("DoorIsOpen", false);
                isOpen = false;
                //close door 
            }

        }
    }

    void open()
    {
        isOpen = true;
    }
}



