using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    public float currentTimeOfDay = 0.35f; // = sabah 8

    bool lockNextDayTrigger = false;

    public List<SkyboxTimeMapping> timeMapping;

    float blendedValue = 0.0f;

    public TextMeshProUGUI timeUI;

    public WeatherSystem weatherSystem;

    public Light light;

    public VolumeProfile volumeProfileDay;
    public VolumeProfile volumeProfileNight;

    public Volume postProcessVolume;

    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;


        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";

        //isigin rotasyonunu guncelleme
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));


        if (weatherSystem.isSpecialWeather == false)
        {
            UpdateSkybox();
        }

        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }

        if (currentHour > 5 && currentHour < 21)
        {
            light.color = new Color(1.0f, 0.956f, 0.839f);
            postProcessVolume.profile = volumeProfileDay;
        }
        else
        {
            light.color = Color.black;
            postProcessVolume.profile = volumeProfileNight;
        }
    }

    private void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMapping)
        {
            if (currentHour == mapping.hour)
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

        if (currentSkybox != null)
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