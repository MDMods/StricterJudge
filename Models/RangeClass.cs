using UnityEngine;
using Decimal = Il2CppSystem.Decimal;

namespace StricterJudge.Models;

internal abstract class RangeClass
{
    private MelonPreferences_Entry<float> _localRange;

    protected RangeClass(string name, string display, MelonPreferences_Category category,
        int minRange, int maxRange, Vector3 offsetHighest, Vector3 offset)
    {
        Name = name;
        DisplayText = display;
        MinRange = minRange;
        MaxRange = maxRange;
        OffsetHighest = offsetHighest;
        Offset = offset;
        CreateEntry(category);
    }

    internal string Name { get; }

    internal string DisplayText { get; }

    internal Decimal RangeDec { get; private set; }

    private Vector3 OffsetHighest { get; }

    private Vector3 Offset { get; }

    private int MinRange { get; }

    private int MaxRange { get; }

    private int LocalRange
    {
        get => (int)_localRange.Value;
        set => _localRange.Value = value;
    }

    private int RangeMs => LocalRange;

    internal string GetDescription() => $"{Name}: {GetRange()}";

    internal Vector3 GetOffset(bool isHighestActive) => isHighestActive
        ? OffsetHighest
        : Offset;

    internal string GetRange() => RangeMs + "ms";

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

    private void CreateEntry(MelonPreferences_Category category)
    {
        _localRange = category.CreateEntry<float>(Name,
            MaxRange,
            description: $"In ms, has to be between {MinRange} and {MaxRange}");
    }
}