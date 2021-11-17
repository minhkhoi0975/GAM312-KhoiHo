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
    // Start weather
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
        if(startWeather)
        {
            PlayNewWeather(startWeather);
        }
    }

    // Remove the old weather and play a new one.
    // If newWeather is null, the method only removes the old weather.
    public void PlayNewWeather(Weather newWeather)
    {
        // References to all characters.
        Character[] characters = null;

        // Remove the effect of the previous weather.
        if (currentWeather != null)
        {
            // Remove the gameplay effect of the previous weather.
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

        // If there is no new weather, skip the remaining steps.
        if (newWeather == null)
        {
            currentWeather = null;
            return;
        }

        // Apply the modifiers of the new weather to the characters.
        // We need to get a new list of characters. Throughout the game, some characters can have been destroyed, and some can have been instantiated.
        characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            foreach (StatModifier statModifier in newWeather.characterStatModifiers)
            {
                character.StatSystem.AddModifier(statModifier);
            }
        }

        // Play the particle effect of the new weather.
        currentWeatherParticleEffect = Instantiate(newWeather.particleEffectPrefab, transform.position, transform.rotation, transform);
        currentWeatherParticleEffect.SetActive(true);

        // Set the current weather to be new weather.
        currentWeather = newWeather;
    }
}
