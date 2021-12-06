/**
 * WeatherEffect.cs
 * Description: This script is used for playing a weather.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectPlayer : MonoBehaviour
{
    // Reference to directional light representing sunlight.
    [SerializeField] Light sunlight;

    // Should the fog be enabled?
    [SerializeField] bool enableFog = true;

    // Start weather
    // If the parameter newWeather of PlayNewWWeather() is null, play this weather.
    [SerializeField] Weather startWeather;

    // Current weather
    Weather currentWeather = null;
    public Weather CurrentWeather
    {
        get
        {
            return currentWeather;
        }
        set
        {
            PlayNewWeather(value);
        }
    }

    // The particle effect of the current weather.
    GameObject currentWeatherParticleEffect;

    private void Start()
    {
        if(!sunlight)
        {
            sunlight = RenderSettings.sun;
        }

        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
        }
        else
        {
            RenderSettings.fog = false;
        }

        if (startWeather)
        {
            PlayNewWeather(startWeather);
        }  
    }

    // Remove the old weather and play a new one.
    // If newWeather is null, play the start weather.
    public void PlayNewWeather(Weather newWeather)
    {
        // References to all characters.
        Character[] characters = null;

        // Remove the effect of the previous weather.
        if (currentWeather != null)
        {
            // Remove the stat modifiers of the previous weather from the characters.
            characters = FindObjectsOfType<Character>();
            foreach (Character character in characters)
            {
                foreach (StatModifier statModifier in currentWeather.characterStatModifiers)
                {
                    character.StatSystem.RemoveAttachedModifier(statModifier);
                }
            }

            // Remove the particle effect of the previous weather.
            if(currentWeatherParticleEffect)
            {
                Destroy(currentWeatherParticleEffect);
            }
        }

        // If there is no new weather, play the start weather.
        if (newWeather == null)
        {
            if (startWeather != null)
            {
                PlayNewWeather(startWeather);
            }
        }
        else
        {
            // Apply the stat modifiers of the new weather to the characters.
            // We need to get a new list of characters. Throughout the game, some characters can have been destroyed, and some can have been instantiated.
            characters = FindObjectsOfType<Character>();
            foreach (Character character in characters)
            {
                foreach (StatModifier statModifier in newWeather.characterStatModifiers)
                {
                    character.StatSystem.AddModifier(statModifier);
                }
            }

            // Modify the sunlight.
            if (sunlight)
            {
                sunlight.color = newWeather.sunlightColor;
                sunlight.intensity = newWeather.sunlightIntensity;
                sunlight.shadowStrength = newWeather.sunlightShadowStrength;
            }

            // Modify the fog.
            if(enableFog)
            {
                RenderSettings.fogDensity = newWeather.fogDensity;
            }

            // Play the particle effect of the new weather.
            if (newWeather.particleEffectPrefab)
            {
                currentWeatherParticleEffect = Instantiate(newWeather.particleEffectPrefab, transform.position, transform.rotation, transform);
                currentWeatherParticleEffect.SetActive(true);
            }

            // Set the current weather to be new weather.
            currentWeather = newWeather;
        }
    }
}
