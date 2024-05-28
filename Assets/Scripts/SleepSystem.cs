using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSystem : MonoBehaviour
{
    public static SleepSystem Instance { get; set; }

    public GameObject sleepUI;

    public GameObject cantSleepPopUp;
    public bool isSleeping;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    public void EnableSleeping()
    {
        sleepUI.SetActive(true);
        isSleeping = true;
    }

    public void DisableSleeping()
    {
        sleepUI.SetActive(false);
        isSleeping = false;
    }

    public void EnableCantSleepPopUp()
    {
        cantSleepPopUp.SetActive(true);
    }

    public void DisableCantSleepPopUp()
    {
        cantSleepPopUp.SetActive(false);
    }
}
