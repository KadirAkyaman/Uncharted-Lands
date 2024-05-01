using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour
{

    private Slider slider;
    public Text fullnesCounter;

    public GameObject playerState;

    private float currentFullnes, maxFullnes;


    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentFullnes = playerState.GetComponent<PlayerState>().currentFullnes;
        maxFullnes = playerState.GetComponent<PlayerState>().maxFullnes;

        float fillValue = currentFullnes / maxFullnes;
        slider.value = fillValue;

        fullnesCounter.text = currentFullnes + "/" + maxFullnes;

    }
}
