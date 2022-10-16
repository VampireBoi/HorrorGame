using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [HideInInspector]
    public ItemData Item;




    public void pick()
    {
        itemSway.instance.holdItem(Item);
        uiManager.instance.closeUI();
    }
}
