using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    void Start()
    {
      
        if (gameObject.name == "block room(Clone)")
        {
            RoomTemplates.Instance.blockRooms.Add(gameObject);
        }
        else { RoomTemplates.Instance.roomList.Add(gameObject); }
        
    }

    
}
