using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    
    bool look;
    GameObject player;  
    // Start is called before the first frame update
    void Start()
    {
        look = false;
        player = GameObject.Find("FootstepsPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (look)
        {
            transform.LookAt(player.transform, Vector3.up);
        }
    }


    public void restRotation()
    {
        look = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void lookAtPlayer()
    {
        look = true;
    }
}
