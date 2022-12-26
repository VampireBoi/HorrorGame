using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadPuzzle : MonoBehaviour
{
    public static KeyPadPuzzle instance;
    public bool canIntract;
    bool puzzlesolved;
    
    public Camera camera;
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
        if(inKeyPadMode && Input.GetKeyDown(KeyCode.Mouse2) && canIntract){
            ExitkeyPadMode();
        }

        if (inKeyPadMode)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
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
        StartCoroutine(enterKeyPad());
    }

    IEnumerator enterKeyPad()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);
        //WaitForSeconds 0.125f 
        inKeyPadMode = true;
        camera.gameObject.SetActive(true);
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    
    public void ExitkeyPadMode()
    {
        StartCoroutine (exitKeyPad());
    }
    IEnumerator exitKeyPad()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

        camera.gameObject.SetActive(false);
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        transform.GetComponent<BoxCollider>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        inKeyPadMode = false;

        if (puzzlesolved)
        {
            Destroy(gameObject);
        }
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
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.playSound("enemy heard somthing");
        yield return new WaitForSeconds(1.2f);

        door.isLocked = false;
        door.tryOpenTheDoor();       
        yield return new WaitForSeconds(1.5f);
        
        GameManager.Instance.level++;
        
        Enemy.instanse.chickForEvent();
        puzzlesolved = true;
        ExitkeyPadMode();
        
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
