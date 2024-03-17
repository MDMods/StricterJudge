namespace StricterJudge.Models;

internal sealed class PerfectRangeClass : RangeClass
{
    internal PerfectRangeClass(string name, string display, MelonPreferences_Category category)
        : base(name, display, category, 1, 50)
    {
    }
}