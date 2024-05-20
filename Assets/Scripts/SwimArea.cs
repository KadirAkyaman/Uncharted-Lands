using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimArea : MonoBehaviour
{
    public GameObject oxygeneBar;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementController>().isSwimming = true;
        }

        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<PlayerMovementController>().isUnderWater = true;
            oxygeneBar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementController>().isSwimming = false;
        }

        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<PlayerMovementController>().isUnderWater = false;
            oxygeneBar.SetActive(false);
            //Reset Oxygen Bar
            PlayerState.Instance.currentOxygenPercent = PlayerState.Instance.maxOxygenPercent;
        }
    }
}
