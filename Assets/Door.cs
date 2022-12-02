using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector]public bool isLocked = true;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        isLocked = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



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
                AudioManager.instance.playSound("close door");
            }
            else
            {
                AudioManager.instance.playSound("open door");
            }
            animator.SetBool("open", !animator.GetBool("open"));
        }
    }
}
