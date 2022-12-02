using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadPuzzle : MonoBehaviour
{
    public static KeyPadPuzzle instance;
    public bool canIntract;

    public Door door;
    [HideInInspector]public bool inKeyPadMode;
    [HideInInspector] public int[] numberCombination = new int[5];

    [HideInInspector] public string password;
    // Start is called before the first frame update


    Ray ray;
    RaycastHit hit;

    void Start()
    {
        instance = this;
        canIntract = true;

        for (int i = 0; i < numberCombination.Length; i++)
        {
            numberCombination[i] = Random.Range(0, 10);
        }
        string s = "";
        for (int i = 0; i < numberCombination.Length; i++)
        {
            s += numberCombination[i] + " ";
            
        }
        password = s;
        Debug.Log(password);


    }

    // Update is called once per frame
    void Update()
    {
        if(inKeyPadMode && Input.GetKeyDown(KeyCode.G)){
            ExitkeyPadMode();
        }

        if (inKeyPadMode)
        {
            ray = transform.GetChild(0).gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "keypad Buttons")
                {
                    if (Input.GetMouseButtonDown(0) && canIntract)
                    {                      
                        hit.transform.gameObject.GetComponent<KeyPadButton>().click();
                    }
                }
                
                
            }
        }
       
    }


    public void enterkeyPadMode()
    {
        inKeyPadMode = true;
        transform.GetChild(0).gameObject.SetActive(true);
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    public void ExitkeyPadMode()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        transform.GetComponent<BoxCollider>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        inKeyPadMode = false;
    }


    public void wrongNumbersCombination()
    {
        Debug.Log("wrongCompniations");
        StartCoroutine(wrongPassword());
        // here we right the code for if the combination is wrong
    }

    public void puzzleSolved()
    {
        
        StartCoroutine(openTheDoor());
        Debug.Log("puzzle solved");
        // here i make the loud sound and then call the checking function

    }
    IEnumerator openTheDoor()
    {
        
        canIntract = false;
        yield return new WaitForSeconds(0.2f);
        KeyPad.instance.LedGreen();

        yield return new WaitForSeconds(1.35f);       
        AudioManager.instance.playSound("door is opened");
        yield return new WaitForSeconds(1.2f);
        
        door.isLocked = false;
        door.tryOpenTheDoor();       
        yield return new WaitForSeconds(1.5f);
        ExitkeyPadMode();
        GameManager.Instance.level++;
        canIntract = true;
        Destroy(gameObject);
    }

    IEnumerator wrongPassword()
    {
        canIntract = false;
        yield return new WaitForSeconds(0.2f);
        KeyPad.instance.Ledred();
        yield return new WaitForSeconds(0.9f);
        door.tryOpenTheDoor();
        yield return new WaitForSeconds(0.5f);
        ExitkeyPadMode();
        canIntract = true;
    }





    public void chickNumbersCombination(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if(numbers[i] != numberCombination[i])
            {
                wrongNumbersCombination();
                return;
            }
        }
        puzzleSolved();

    }
}
