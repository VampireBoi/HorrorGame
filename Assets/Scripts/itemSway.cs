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


        
    }


}
