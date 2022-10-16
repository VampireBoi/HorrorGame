using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSway : MonoBehaviour
{
    [Header("sway settings")]
    [SerializeField] 
    private float smooth;
    [SerializeField] 
    private float swayMultiplier;
    
    [HideInInspector]
    public ItemData ItemInHand;
    public static itemSway instance;


    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        //spown the first item in the invenotry 
 
        //get the mouse input 
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        // calculate target rotation 

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (transform.childCount != 0)
            {
                throwItem();
            }
        }
    }

    public void holdItem(ItemData item)
    {

        destroyItemInHand();

        // instantiate the object in the plyer hand
        var obj = Instantiate(item.prefab, transform.position, transform.rotation);
        Rigidbody rb; 
        obj.transform.TryGetComponent<Rigidbody>(out rb);
        if(rb != null)
        {
            Destroy(rb);
        }
        ItemInHand = item;
        obj.transform.parent = transform;
    }
    public void throwItem()
    {
        Instantiate(ItemInHand.prefab, transform.position, Quaternion.identity);
        destroyItemInHand();
        InventoryManager.Instance.items.Remove(ItemInHand);
        ItemInHand = null;
    }

    public void destroyItemInHand()
    {
        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public void holdFirstItem()
    {
        if (InventoryManager.Instance.items.Count != 0 && transform.childCount == 0 && InventoryManager.Instance.items.Count < 2)
        {
            holdItem(InventoryManager.Instance.items[0]);
        }
    }


}
