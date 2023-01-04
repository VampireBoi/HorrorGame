using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorCranck : MonoBehaviour
{
    public Generator generator;
    public Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AddPowerToGenerator()
    {
        generator.addPower();
        animator.SetTrigger("spin");
        AudioManager.instance.playSound("crank Sound");
    }
}
