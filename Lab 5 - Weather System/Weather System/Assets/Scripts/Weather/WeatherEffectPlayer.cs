/**
 * WeatherEffect.cs
 * Description: This script is used for configuring a weather effect.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectPlayer : MonoBehaviour
{
    // The particle effect of the weather.
    [SerializeField] GameObject particleEffectPrefab;
    public GameObject ParticleEffectPrefab
    {
        get
        {
            return particleEffectPrefab;
        }
        set
        {
            particleEffectPrefab = value;
            PlayWeatherEffect();
        }
    }

    // Particle effect of current weather.
    GameObject currentParticleEffect;

    private void Start()
    {
        if(particleEffectPrefab)
        {
            PlayWeatherEffect();
        }
    }

    void PlayWeatherEffect()
    {
        // Remove the previous particle effect.
        if(currentParticleEffect)
        {
            Destroy(currentParticleEffect);
        }

        if (!particleEffectPrefab)
            return;

        // Instantiate new particle effect.
        currentParticleEffect = Instantiate(particleEffectPrefab, transform.position, transform.rotation, transform);
        currentParticleEffect.SetActive(true);
    }
}
