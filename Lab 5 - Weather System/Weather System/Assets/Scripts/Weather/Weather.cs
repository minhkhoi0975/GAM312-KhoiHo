/**
 * Weather.cs
 * Description: This script defines a weather, specifically its particle effect and how it affects character's stats.
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

    // How the weather affects characters.
    // Make sure that the modifiers' type is set to Attached. Otherwise, the modifiers will pernamently affect the characters.
    public List<StatModifier> characterStatModifiers;
}
