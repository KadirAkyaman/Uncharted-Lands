using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygeneBar : MonoBehaviour
{
    private Slider slider;
    public Text oxygenCounter;

    private float currentOxygen, maxOxygen;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    private void Update() {
        currentOxygen = PlayerState.Instance.currentOxygenPercent;

        maxOxygen = PlayerState.Instance.maxOxygenPercent;

        float fillValue = currentOxygen / maxOxygen;

        slider.value = fillValue;

        oxygenCounter.text = currentOxygen + "%";
    }
}
