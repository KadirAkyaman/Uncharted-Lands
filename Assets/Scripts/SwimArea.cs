using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimArea : MonoBehaviour
{
    public GameObject oxygeneBar;
    public GameObject normalPostProcess;

    public AudioSource waterChannel;
    public AudioClip underWaterSound;

    private void Start()
    {
        StartCoroutine(ChangeWaterVolume());
        
    }
    IEnumerator ChangeWaterVolume()
    {
        waterChannel.volume = 0;
        yield return new WaitForSeconds(5);
        waterChannel.volume = 0.6f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementController>().isSwimming = true;
        }

        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<PlayerMovementController>().isUnderWater = true;
            normalPostProcess.SetActive(false);
            oxygeneBar.SetActive(true);
        }

        if (waterChannel.isPlaying == false)
        {
            waterChannel.clip = underWaterSound;
            waterChannel.loop = true;
            waterChannel.Play();
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
            normalPostProcess.SetActive(true);
            oxygeneBar.SetActive(false);
            //Reset Oxygen Bar
            PlayerState.Instance.currentOxygenPercent = PlayerState.Instance.maxOxygenPercent;
        }

        if (waterChannel.isPlaying)
        {
            waterChannel.Stop();
        }
    }
}
