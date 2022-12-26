using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTv : MonoBehaviour
{
    public static OldTv instance;
    [Header("time for the distraction")]
    public float distractionTime;
    public float timer;
    [HideInInspector] public bool CanCloseTv;
    [HideInInspector] public bool CanOpenTv;
    [HideInInspector] public bool TvOn;
    [Header("the amount of times you can use the tv before it breaks")]
    public int numberOfUse;
    
    void Start()
    {
        instance = this;
        CanCloseTv = true;
        CanOpenTv = true;
        timer = distractionTime;
       
    }

    private void Update()
    {
        if (Enemy.instanse.isDistracted && TvOn)
        {
            if (timer <= 0)
            {
                closeTv();
                timer = distractionTime;
            }
            else { timer -= Time.deltaTime; }
        }
    }

    public void openTv()
    {
        if (CanOpenTv)
        {
            numberOfUse--;
            Enemy.instanse.CreateDistraction();
            timer = distractionTime;
            TvOn = true;
        }
    }

    public void closeTv()
    {
        if (CanCloseTv)
        {
            Enemy.instanse.stopDistraction();
            TvOn = false;
        }
    }
}
