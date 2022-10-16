using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new List<ItemData>();

    // the ui grid that is going to display the item holders  
    public Transform itemContent;
    
    // the item holder in the inventory ui
    public GameObject itemPrefab;


    private void Start()
    {
        Instance = this;
    }


    public void addItem(ItemData item) {
        items.Add(item);
    }

    public void removeItem(ItemData item)
    {
        items.Remove(item); 
    }


    // every time the player open the inventory this function get called , it take the items in the list and disply them in the item conent ui 
    public void listItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (ItemData item in items) 
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            var itemName = obj.transform.Find("item name").GetComponent<Text>();
            var itemimage = obj.transform.Find("Image").GetComponent<Image>();
            ItemController itemController = obj.GetComponent<ItemController>();
            itemController.Item = item;
            itemimage.sprite = item.icon;
            itemName.text = item.name;
        }
    }
}
