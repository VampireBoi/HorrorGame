using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpowner : MonoBehaviour
{
    public int openingDiration;
    int rand;
    bool spawned = false;
    // 1 --> need down door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    // Start is called before the first frame update
    void Start()
    {
        Invoke("spown", 0.2f);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void spown()
    {
        if(spawned == false)
        {
            if(LevelManager.instance.timeForSpawning >= 0)
            {
                if (openingDiration == 1)
                {
                    rand = Random.Range(0, RoomTemplates.Instance.bottomRooms.Length);
                    Instantiate(RoomTemplates.Instance.bottomRooms[rand], transform.position, RoomTemplates.Instance.bottomRooms[rand].transform.rotation);

                }
                else if (openingDiration == 2)
                {
                    rand = Random.Range(0, RoomTemplates.Instance.topRooms.Length);
                    Instantiate(RoomTemplates.Instance.topRooms[rand], transform.position, RoomTemplates.Instance.topRooms[rand].transform.rotation);

                }
                else if (openingDiration == 3)
                {
                    rand = Random.Range(0, RoomTemplates.Instance.leftRooms.Length);
                    Instantiate(RoomTemplates.Instance.leftRooms[rand], transform.position, RoomTemplates.Instance.leftRooms[rand].transform.rotation);

                }
                else if (openingDiration == 4)
                {
                    rand = Random.Range(0, RoomTemplates.Instance.RightRooms.Length);
                    Instantiate(RoomTemplates.Instance.RightRooms[rand], transform.position, RoomTemplates.Instance.RightRooms[rand].transform.rotation);
                }
            }
            else { Instantiate(RoomTemplates.Instance.blockRoom, transform.position, RoomTemplates.Instance.blockRoom.transform.rotation); }
            

            spawned = true;
        }
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpowner roomSpowner;
            bool found = other.TryGetComponent<RoomSpowner>(out roomSpowner);
            if (found && roomSpowner.spawned == false && spawned == false)
            {
                Instantiate(RoomTemplates.Instance.blockRoom, transform.position, RoomTemplates.Instance.blockRoom.transform.rotation);
                Destroy(gameObject);
            }
            spawned = true;
            
        }
    }
}
