using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currentTimeOfDay = 0.35f; // = sabah 8

    bool lockNextDayTrigger = false;

    public List<SkyboxTimeMapping> timeMapping;

    float blendedValue = 0.0f;

    public TextMeshProUGUI timeUI;
    
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;


        currentHour = Mathf.FloorToInt(currentTimeOfDay*24);

        timeUI.text = $"{currentHour}:00";

        //isigin rotasyonunu guncelleme
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay*360)- 90, 170, 0));

        //skybox materyalini guncelleme
        UpdateSkybox();
    }

    private void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMapping)
        {
            if(currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if (currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);

                        currentSkybox.SetFloat("_TransitionFactor", blendedValue);
                    }
                    else
                    {
                        blendedValue = 0;
                    }
                }



                break;
            }
        }
        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        if (currentHour!=0)
        {
            lockNextDayTrigger = false;
        }


        if (currentSkybox!=null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}