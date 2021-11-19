/**
 * Weather.cs
 * Description: This script defines a weather, specifically its particle effect, how it affects sunlight, how it affects fog, and how it affects character's stats.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weather", menuName = "Weather/Create a new Weather")]
public class Weather : ScriptableObject
{
    // The particle effect of the weather.
    public GameObject particleEffectPrefab;

    // Sunlight color, intensity, and shadow strength.
    public Color sunlightColor = new Color(255.0f, 244.0f, 214.0f, 255.0f);
    public float sunlightIntensity = 1.0f;
    public float sunlightShadowStrength = 1.0f;

    // Fog color and intensity.
    public Color fogColor = new Color(128.0f, 128.0f, 128.0f, 0.0f);
    public float fogDensity = 0.0f;

    // How the weather affects characters.
    // Make sure that the modifiers' type is set to Attached. Otherwise, the modifiers will pernamently affect the characters.
    public List<StatModifier> characterStatModifiers;  
}
