using UnityEngine;

namespace StricterJudge.Models;

internal sealed class PerfectRangeClass : RangeClass
{
    internal PerfectRangeClass(string name, string display, MelonPreferences_Category category, Vector3 highestOffset,
        Vector3 offset)
        : base(name, display, category, 1, 50, highestOffset, offset) { }
}