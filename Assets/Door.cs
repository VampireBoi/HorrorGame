using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector]public bool isLocked = true;
    Animator animator;

    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        isLocked = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void tryOpenTheDoor()
    {
        if (isLocked)
        {
            AudioManager.instance.playSound("door is closed");
            animator.SetTrigger("can't open");
        }
        else
        {
            if (animator.GetBool("open"))
            {
                isOpen=false;
                AudioManager.instance.playSound("close door");
            }
            else
            {
                isOpen = true;
                AudioManager.instance.playSound("open door");
            }
            animator.SetBool("open", !animator.GetBool("open"));
        }
    }
}
