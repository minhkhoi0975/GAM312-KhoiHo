using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class Weather : MonoBehaviour
{
    string url = "http://api.openweathermap.org/data/2.5/weather?q=hooksett,nh,usa&appid=c9dc7afdbcda9f01bca38d18f699f0eb";

    [SerializeField] string cityName = "windham";
    [SerializeField] string stateCode = "nh";
    [SerializeField] string countryCode = "us";

    [SerializeField] string apiKey = "c9dc7afdbcda9f01bca38d18f699f0eb";

    // Use this for initialization
    void Start()
    {
        url = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "," + stateCode + "," + countryCode + "&appid=" + apiKey;

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
            }
        }
    }
}
