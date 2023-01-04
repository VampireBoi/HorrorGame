using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public float powerDrainSpeed;
    public float AmountOfPowerAdded;
    public float power;
    public Slider powerUi;
    
    public Animator generatorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        power = Mathf.Clamp(power, 0f, 1f);
        powerUi.value = power;

        if(power > 0.05f)
        {
            generatorAnimator.SetBool("thereIsPower", true);
            AudioManager.instance.playSound("generator Sound");
        } else
        {
            generatorAnimator.SetBool("thereIsPower", false);
            AudioManager.instance.stopSound("generator Sound");

        }

        power -= Time.deltaTime * powerDrainSpeed;
    }



    public void addPower()
    {
        power += AmountOfPowerAdded;
    }





}
