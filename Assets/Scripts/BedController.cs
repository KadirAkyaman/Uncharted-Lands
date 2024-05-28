using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;

    public GameObject dayNightSystem;

    
    private void Start() 
    {
        dayNightSystem = GameObject.Find("DayNightSystem");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.SelectedObject == gameObject) // eger e tusuna basilirsa objeyi envantere ekle
        {

            if(dayNightSystem.GetComponent<DayNightSystem>().currentHour >= 21)
            {
                StartCoroutine(SleepToNextDay());
            }
            else if (dayNightSystem.GetComponent<DayNightSystem>().currentHour >= 0 && dayNightSystem.GetComponent<DayNightSystem>().currentHour <= 6)
            {
                StartCoroutine(SleepToMorning());
            }
            else
            {
                StartCoroutine(CantSleepPopUp());
            }
        }
    }


    IEnumerator SleepToNextDay()
    {
        SleepSystem.Instance.EnableSleeping();
        dayNightSystem.GetComponent<DayNightSystem>().currentTimeOfDay = 0.3f;
        TimeManager.Instance.TriggerNextDay();
        yield return new WaitForSeconds(4f);
        SleepSystem.Instance.DisableSleeping();
    }
    IEnumerator SleepToMorning()
    {
        SleepSystem.Instance.EnableSleeping();
        dayNightSystem.GetComponent<DayNightSystem>().currentTimeOfDay = 0.3f;
        yield return new WaitForSeconds(4f);
        SleepSystem.Instance.DisableSleeping();
    }

    IEnumerator CantSleepPopUp()
    {
        SleepSystem.Instance.EnableCantSleepPopUp();
        yield return new WaitForSeconds(2f);
        SleepSystem.Instance.DisableCantSleepPopUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public string GetItemName()
    {
        return ItemName;
    }
}
