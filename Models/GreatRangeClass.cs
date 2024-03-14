using MelonLoader;

namespace StricterJudge.Models;

internal sealed class GreatRangeClass : RangeClass
{
    internal GreatRangeClass(string name, string display, MelonPreferences_Category category)
        : base(name, display, category, 0, 80)
    {
    }
}