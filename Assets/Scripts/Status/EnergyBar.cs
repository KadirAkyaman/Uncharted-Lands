using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    private Slider slider;

    public GameObject playerState;

    private float currentEnergy, maxEnergy;


    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentEnergy = playerState.GetComponent<PlayerState>().currentEnergy;
        maxEnergy = playerState.GetComponent<PlayerState>().maxEnergy;

        float fillValue = currentEnergy / maxEnergy;
        slider.value = fillValue;

    }
}
