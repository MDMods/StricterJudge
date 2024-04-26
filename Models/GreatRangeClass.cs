using UnityEngine;

namespace StricterJudge.Models;

internal sealed class GreatRangeClass : RangeClass
{
    internal GreatRangeClass(string name, string display, MelonPreferences_Category category, Vector3 highestOffset,
        Vector3 offset)
        : base(name, display, category, 0, 80, highestOffset, offset) { }
}