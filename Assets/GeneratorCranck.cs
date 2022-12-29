using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorCranck : MonoBehaviour
{
    public Generator generator;
  
    public void AddPowerToGenerator()
    {
        generator.addPower();
    }
}
