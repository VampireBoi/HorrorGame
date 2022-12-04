using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTv : MonoBehaviour
{
    public static OldTv instance;
    [HideInInspector] public bool CanCloseTv;
    [HideInInspector] public bool CanOpenTv;
    [HideInInspector] public bool TvOn;
    void Start()
    {
        instance = this;
        CanCloseTv = true;
        CanOpenTv = true;
    }

    public void openTv()
    {
        if (CanOpenTv)
        {
            Enemy.instanse.CreateDistraction();
        }
    }

    public void closeTv()
    {
        if (CanCloseTv)
        {
            Enemy.instanse.stopDistraction();
        }
    }
}
