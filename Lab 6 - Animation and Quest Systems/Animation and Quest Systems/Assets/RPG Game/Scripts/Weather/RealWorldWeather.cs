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

[System.Serializable]
public class WeatherConditionCode
{
    public string code;
    public Weather weather;
}

public class RealWorldWeather : MonoBehaviour
{
    string url = "http://api.openweathermap.org/data/2.5/weather?q=windham,nh,usa&appid=c9dc7afdbcda9f01bca38d18f699f0eb";

    [SerializeField] string cityName = "windham";
    [SerializeField] string stateCode = "nh";
    [SerializeField] string countryCode = "us";

    [SerializeField] string apiKey = "c9dc7afdbcda9f01bca38d18f699f0eb";

    // Reference to component that plays weather effect.
    [SerializeField] WeatherEffectPlayer weatherEffectPlayer;

    // List of weather condition codes and their respective weather effects.
    [SerializeField] List<WeatherConditionCode> weatherConditonCodes;

    // weatherConditionCodes in form of dictionary.
    Dictionary<string, Weather> weatherConditionsDict = new Dictionary<string, Weather>();

    private void Awake()
    {
        foreach(WeatherConditionCode weatherConditionCode in weatherConditonCodes)
        {
            weatherConditionsDict[weatherConditionCode.code] = weatherConditionCode.weather;
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

                string weatherCode = data["weather"][0]["main"];

                Debug.Log("Weather: " + weatherCode);

                if (weatherConditionsDict.ContainsKey(weatherCode))
                {
                    weatherEffectPlayer.PlayNewWeather(weatherConditionsDict[weatherCode]);
                }
                else
                {
                    Debug.LogWarning("Cannot play this weather: " + weatherCode);
                }
            }
        }
    }
}
