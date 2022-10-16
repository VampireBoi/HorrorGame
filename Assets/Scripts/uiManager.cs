using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiManager : MonoBehaviour
{  
    public static uiManager instance;
    public GameObject inventoryUI;
    public bool inventoryIsOpen = false;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Interact.isSetting)
        {
            InventoryManager.Instance.listItems();
            Cursor.lockState = CursorLockMode.Confined;
            inventoryUI.SetActive(true);
            inventoryIsOpen = true;
        }

    }

    public void closeUI()
    {
        inventoryIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI?.SetActive(false);
    }
}
