using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [HideInInspector]
    public ItemData ItemInHand;
    public static ItemHolder instance;
    public float amountOfForce;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (rb != null)
        {
            Destroy(rb);
        }
        ItemInHand = item;
        obj.transform.parent = transform;
    }
    public void throwItem()
    {
        GameObject o = Instantiate(ItemInHand.prefab, transform.position, Quaternion.identity);
        o.GetComponent<Rigidbody>().AddForce(transform.forward * amountOfForce);
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
