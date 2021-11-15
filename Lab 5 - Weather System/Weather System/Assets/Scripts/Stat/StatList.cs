/**
 * StatList.cs
 * Description: Each instance of this class contains a list of stats.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat List", menuName = "Stats/Create a new Stat List")]
public class StatList : ScriptableObject
{
    public List<Stat> stats = new List<Stat>();
}
