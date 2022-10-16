using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public static RoomTemplates Instance;
   
     
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] RightRooms;

    public GameObject blockRoom;

    public GameObject key;

    public List<GameObject> roomList;

    public List<GameObject> blockRooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
        
    }  
}

