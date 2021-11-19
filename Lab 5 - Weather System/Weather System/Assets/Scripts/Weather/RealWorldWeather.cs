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
    Clouds,
    Rain,
    Snow,
    Extreme
}

public class RealWorldWeather : MonoBehaviour
{
    string url = "http://api.openweathermap.org/data/2.5/weather?q=windham,nh,usa&appid=c9dc7afdbcda9f01bca38d18f699f0eb";

    [SerializeField] string cityName = "windham";
    [SerializeField] string stateCode = "nh";
    [SerializeField] string countryCode = "us";

    [SerializeField] string apiKey = "c9dc7afdbcda9f01bca38d18f699f0eb";

    public WeatherEffectPlayer weatherEffectPlayer;
    [SerializeField] Weather clear;
    [SerializeField] Weather clouds;
    [SerializeField] Weather rain;
    [SerializeField] Weather snow;
    [SerializeField] Weather extreme;

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
                    case "Clear":
                        currentWeatherType = WeatherType.Clear;
                        weatherEffectPlayer.PlayNewWeather(clear);
                        break;

                    case "Clouds":
                        currentWeatherType = WeatherType.Clouds;
                        weatherEffectPlayer.PlayNewWeather(clouds);
                        break;

                    case "Rain":
                        currentWeatherType = WeatherType.Rain;
                        weatherEffectPlayer.PlayNewWeather(rain);
                        break;

                    case "Snow":
                        currentWeatherType = WeatherType.Snow;
                        weatherEffectPlayer.PlayNewWeather(snow);
                        break;

                    case "Extreme":
                        currentWeatherType = WeatherType.Extreme;
                        weatherEffectPlayer.PlayNewWeather(extreme);
                        break;

                    default:
                        currentWeatherType = WeatherType.Clear;
                        weatherEffectPlayer.PlayNewWeather(clear);
                        break;
                }
            }
        }
    }
}
