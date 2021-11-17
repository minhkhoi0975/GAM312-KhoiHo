/**
 * Weather.cs
 * Description: This script gets infromation about current weather in a city from Open Weather Map (http://api.openweathermap.org).
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public enum WeatherType
{
    Clear,
    Rain,
    Snow,
    Extreme
}

public class Weather : MonoBehaviour
{
    string url = "http://api.openweathermap.org/data/2.5/weather?q=windham,nh,usa&appid=c9dc7afdbcda9f01bca38d18f699f0eb";

    [SerializeField] string cityName = "windham";
    [SerializeField] string stateCode = "nh";
    [SerializeField] string countryCode = "us";

    [SerializeField] string apiKey = "c9dc7afdbcda9f01bca38d18f699f0eb";

    public WeatherEffectPlayer weatherEffectPlayer;
    [SerializeField] GameObject rainParticleEffect;
    [SerializeField] GameObject snowParticleEffect;
    [SerializeField] GameObject extremeParticleEffect;

    WeatherType currentWeatherType = WeatherType.Clear;
    public WeatherType CurrentWeatherType
    {
        get
        {
            return currentWeatherType;
        }
    }

    // Use this for initialization
    void Start()
    {
        url = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName;
        if(stateCode != "")
        {
            url += "," + stateCode;
        }
        if(countryCode != "")
        {
            url += "," + countryCode;
        }
        url += "&appid=" + apiKey;

        StartCoroutine(GetWeather());
    }
    IEnumerator GetWeather()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if(webRequest.isNetworkError)
            {
                Debug.LogError("Network error.");
            }
            else if(webRequest.isHttpError)
            {
                Debug.LogError("HTTP error.");
            }
            else
            {
                // Get the text from the web page.
                string websiteText = webRequest.downloadHandler.text;

                // Get info about weather.
                SimpleJSON.JSONNode data = SimpleJSON.JSON.Parse(websiteText);

                Debug.Log("Weather: " + data["weather"][0]["main"]);

                switch (data["weather"][0]["main"])
                {
                    case "Rain":
                        currentWeatherType = WeatherType.Clear;
                        weatherEffectPlayer.ParticleEffectPrefab = rainParticleEffect;
                        break;

                    case "Snow":
                        currentWeatherType = WeatherType.Snow;
                        weatherEffectPlayer.ParticleEffectPrefab = snowParticleEffect;
                        break;

                    case "Extreme":
                        currentWeatherType = WeatherType.Extreme;
                        weatherEffectPlayer.ParticleEffectPrefab = extremeParticleEffect;
                        break;

                    default:
                        currentWeatherType = WeatherType.Clear;
                        weatherEffectPlayer.ParticleEffectPrefab = null;
                        break;
                }
            }
        }
    }
}
