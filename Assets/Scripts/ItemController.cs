using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [HideInInspector]
    public ItemData Item;




    public void pick()
    {
        ItemHolder.instance.holdItem(Item);
        uiManager.instance.closeUI();
    }
}
