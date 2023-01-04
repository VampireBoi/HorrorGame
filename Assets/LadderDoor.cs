using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDoor : MonoBehaviour
{
    public GameObject ladder;
    [HideInInspector] public static bool doorIsOpen;

    private void Start()
    {
        doorIsOpen = false;
        ladder.SetActive(doorIsOpen);
    }
    public void intract()
    {
        if (!doorIsOpen)
        {
            ladder.SetActive(true);
            doorIsOpen = true;
        }
        else
        {
            ladder.SetActive(false);
            doorIsOpen = false;
        }
    }
}
