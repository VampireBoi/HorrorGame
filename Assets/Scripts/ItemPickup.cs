using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    public void pickup()
    {  
        InventoryManager.Instance.addItem(item);
        itemSway.instance.holdFirstItem();
        Destroy(gameObject);
    }
}
