using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadButton : MonoBehaviour
{
    public int id;
    public void click()
    {
        KeyPad.instance.addNumber(id);
    }
}
