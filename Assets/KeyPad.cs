using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    public static KeyPad instance;
    bool animateLight;
    int[] numberCombination = new int[5];
    int i = 0;

    public GameObject orangeLight;
    public GameObject redLight;
    public GameObject greenLight;

    // Start is called before the first frame update
    void Start()
    {     
        animateLight = true;
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        if(i == 5)
        {
            KeyPadPuzzle.instance.chickNumbersCombination(numberCombination);
            Array.Clear(numberCombination, 0, numberCombination.Length);
            i = 0;
        }
    }


    public void addNumber(int n)
    {  
        numberCombination[i] = n;
        i++;
        if (animateLight)
        {         
            StartCoroutine(lightAnimPress(orangeLight));
            animateLight = false;
        }
    }

    public void LedGreen() {
        if (animateLight)
        {
            StartCoroutine(ComparePasswardGreen(greenLight));
            animateLight = false;
        }
    }

    public void Ledred()
    {
        if (animateLight)
        {
            StartCoroutine(ComparePasswardred(redLight));
            animateLight = false;
        }
    }

    IEnumerator lightAnimPress(GameObject light)
    {

        AudioManager.instance.changePitch("beep sound", 1);
        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        animateLight = true;
    }


    
    
    
    IEnumerator ComparePasswardGreen(GameObject light)
    {
        AudioManager.instance.changePitch("beep sound", 0.4f);
        AudioManager.instance.changeVolume("beep sound", 1);
        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");      
        yield return new WaitForSeconds(0.3f);
        
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);

        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);

        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");      
        animateLight = true;
    }

    IEnumerator ComparePasswardred(GameObject light)
    {
        AudioManager.instance.changePitch("beep sound", 0.8f);

        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);

        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);

        AudioManager.instance.playSound("beep sound");
        light.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        
        light.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        animateLight = true;
    }






}
