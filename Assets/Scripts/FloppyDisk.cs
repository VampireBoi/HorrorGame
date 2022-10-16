using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppyDisk : MonoBehaviour
{
    public FloppyDeskData disk;


    private void Awake()
    {
        disk.isFinished = false;
    }
    private void Update()
    {
        if (disk.isFinished)
        {
            Destroy(gameObject);
            InventoryManager.Instance.items.Remove(transform.GetComponent<ItemPickup>().item);
        }
    }
}
