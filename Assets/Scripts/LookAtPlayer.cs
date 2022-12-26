using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    
    bool lookatPlayer;
    bool LookatTheRoom;
    GameObject player;
    public GameObject lookPoint;

    // Start is called before the first frame update
    void Start()
    {
        lookatPlayer = false;
        LookatTheRoom = false;
        player = GameObject.Find("FootstepsPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("lookatPlayer: " + lookatPlayer);
        //Debug.Log("lookatTheRoom: " + LookatTheRoom);

        if (lookatPlayer)
        {
            transform.LookAt(player.transform, Vector3.up);
        }

        if (LookatTheRoom)
        {
            transform.LookAt(lookPoint.transform, Vector3.up);
        }
    }


    public void restRotation()
    {
        lookatPlayer = false;
        LookatTheRoom = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void lookAtPlayer()
    {
        LookatTheRoom = false;
        lookatPlayer = true;
    }

    public void LookAtTheRoom()
    {
        LookatTheRoom = true;
        lookatPlayer = false;
        lookPoint.GetComponent<Animator>().SetTrigger("chick");
    }
}
