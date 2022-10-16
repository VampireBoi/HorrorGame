using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{ 
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    private void Start()
    {  
        gameObject.SetActive(false);
        textComponent.text = string.Empty;     
    }

    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            NextLine();
        }
    }




    public void startDialogue()
    {
        gameObject.SetActive(true);
        index = 0;
        StartCoroutine(typeLine());
    }
    IEnumerator typeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(typeLine());
        }
        else
        {
            Destroy(gameObject);  
        }
    }
}
