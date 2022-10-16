using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    public void pickup()
    {
        
        InventoryManager.Instance.items.Add(item);
        itemSway.instance.holdFirstItem();
        Destroy(gameObject);
    }
}
