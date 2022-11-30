using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTv : MonoBehaviour
{
    public static OldTv instance;
    void Start()
    {
        instance = this;
    }

    
    void Update()
    {
        
    }


    public void openTv()
    {
        Enemy.instanse.CreateDistraction();
    }

    public void closeTv()
    {
        Enemy.instanse.stopDistraction();
    }
}
