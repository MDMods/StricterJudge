using UnityEngine;
using Decimal = Il2CppSystem.Decimal;

namespace StricterJudge.Models;

internal abstract class RangeClass
{
    private MelonPreferences_Entry<float> _localRange;

    protected RangeClass(string name, string display, MelonPreferences_Category category,
        int minRange, int maxRange)
    {
        Name = name;
        DisplayText = display;
        MinRange = minRange;
        MaxRange = maxRange;
        CreateEntry(category);
    }

    private int MinRange { get; }

    private int MaxRange { get; }

    internal string Name { get; }

    internal string DisplayText { get; }

    private int LocalRange
    {
        get => (int)_localRange.Value;
        set => _localRange.Value = value;
    }

    private int RangeMs => LocalRange;

    internal Decimal RangeDec { get; private set; }

    private void CreateEntry(MelonPreferences_Category category)
    {
        _localRange = category.CreateEntry<float>(Name,
            MaxRange,
            description: $"In ms, has to be between {MinRange} and {MaxRange}");
    }

    internal void InitEntryValue()
    {
        var originalRange = LocalRange;
        LocalRange = Mathf.Clamp(LocalRange, MinRange, MaxRange);

        RangeDec = (Decimal)RangeMs / 1000;

        if (originalRange == LocalRange) return;

        var warningMessage = $"Your selected range for {Name} is out of bounds.";
        warningMessage += $"The value has to be between {MinRange} and {MaxRange}.";
        MelonLogger.Warning(warningMessage);
    }

    internal string GetRange() => RangeMs + "ms";

    internal string GetDescription() => $"{Name}: {GetRange()}";
}