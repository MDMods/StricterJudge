using MelonLoader;
using UnityEngine;
using Decimal = Il2CppSystem.Decimal;

namespace StricterJudge.Models;

internal abstract class RangeClass
{
    private MelonPreferences_Entry<float> _localRange;

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

    protected RangeClass(string name, string display, MelonPreferences_Category category,
        int minRange, int maxRange)
    {
        Name = name;
        DisplayText = display;
        MinRange = minRange;
        MaxRange = maxRange;
        LoadEntry(category);
    }

    private void BaseRangeEntry(MelonPreferences_Category category, int minRange, int maxRange)
    {
        _localRange = category.CreateEntry<float>(Name,
            maxRange,
            description: $"In ms, has to be between {minRange} and {maxRange}");
    }

    private void InitEntryValue(int minRange, int maxRange)
    {
        var originalRange = LocalRange;
        LocalRange = Mathf.Clamp(LocalRange, minRange, maxRange);

        RangeDec = (Decimal)RangeMs / 1000;

        if (originalRange == LocalRange)
        {
            return;
        }

        var warningMessage = $"Your selected range for {Name} is out of bounds.";
        warningMessage += $"The value has to be between {minRange} and {maxRange}.";
        MelonLogger.Warning(warningMessage);
    }

    private void LoadEntry(MelonPreferences_Category category)
    {
        BaseRangeEntry(category, MinRange, MaxRange);
        InitEntryValue(MinRange, MaxRange);
    }

    internal string GetRange() => RangeMs + "ms";

    internal string GetDescription() => $"{Name}: {GetRange()}";
}