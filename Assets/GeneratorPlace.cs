using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorPlace : MonoBehaviour
{
    public GameObject generator;
    public Generator generatorInstance;
    public static GeneratorPlace instance;
    public bool hideBoxColider;
    public GameObject generatorHighlight;
    public bool showHighLight;
    public bool thereIsGenerator;

    BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        instance = this;
        showHighLight = false;
        thereIsGenerator = false;
        generatorHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (ItemHolder.instance.transform.childCount > 0 && ItemHolder.instance.transform.GetChild(0).name == "generator item(Clone)")
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }



        if (showHighLight)
        {
            generatorHighlight.SetActive(true);
        }
        else
        {
            generatorHighlight.SetActive(false);
        }
     
    }

    public void spownGenerator()
    {
        ItemHolder.instance.destroyItemInHand();
        InventoryManager.Instance.items.Remove(ItemHolder.instance.ItemInHand);
        ItemHolder.instance.ItemInHand = null;
        GameObject g = Instantiate(generator);
        g.transform.position = transform.GetChild(0).transform.position;
        g.transform.rotation = transform.GetChild(0).transform.rotation;
        thereIsGenerator = true;
        g.transform.Find("cranck").GetComponent<BoxCollider>().enabled = true;
        generatorInstance = g.GetComponent<Generator>();



    }




    


}
