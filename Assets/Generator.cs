using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public float powerDrainSpeed;
    public float AmountOfPowerAdded;
    float power;
    public Slider powerUi;
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



        power -= Time.deltaTime * powerDrainSpeed;
    }



    public void addPower()
    {
        power += AmountOfPowerAdded;
    }





}
