using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [Range(0f, 1f)]
    public float changeToRainSpring = 0.3f;

    [Range(0f, 1f)]
    public float changeToRainSummer = 0.0f;

    [Range(0f, 1f)]
    public float changeToRainFall = 0.4f;

    [Range(0f, 1f)]
    public float changeToRainWinter = 0.75f;

    public GameObject rainEffect;
    public Material rainSkybox;

    public bool isSpecialWeather;

    public AudioSource rainChannel;
    public AudioClip rainSound;

    public enum WeatherConditions
    {
        Sunny,
        Rainy
    }

    private WeatherConditions currentWeather = WeatherConditions.Sunny;


    private void Start()
    {
        TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);
    }

    private void GenerateRandomWeather()
    {
        TimeManager.Season currentSeason = TimeManager.Instance.currentSeason;

        float changeToRain = 0f;

        switch (currentSeason)
        {
            case TimeManager.Season.Spring:
                changeToRain = changeToRainSpring;
                break;

            case TimeManager.Season.Summer:
                changeToRain = changeToRainSummer;
                break;

            case TimeManager.Season.Fall:
                changeToRain = changeToRainFall;
                break;

            case TimeManager.Season.Winter:
                changeToRain = changeToRainWinter;
                break;
        }

        if (Random.value <= changeToRain)
        {
            currentWeather = WeatherConditions.Rainy;
            isSpecialWeather = true;



            Invoke("StartRain", 1f);
        }
        else
        {
            currentWeather = WeatherConditions.Sunny;
            isSpecialWeather = false;

            StopRain();
        }
    }

    private void StartRain()
    {
        if (rainChannel.isPlaying == false)
        {
            rainChannel.clip = rainSound;
            rainChannel.loop = true;
            rainChannel.Play();
        }

        RenderSettings.skybox = rainSkybox;
        rainEffect.SetActive(true);
    }

    private void StopRain()
    {
        if (rainChannel.isPlaying)
        {
            rainChannel.Stop();
        }

        rainEffect.SetActive(false);
    }
}
