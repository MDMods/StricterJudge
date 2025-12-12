using MelonLoader.Preferences;
using UnityEngine;
using Decimal = Il2CppSystem.Decimal;

namespace StricterJudge.Models;

internal class JudgementRange
{
    private MelonPreferences_Entry<float> _localRange;

    private JudgementRange(
        string name,
        string display,
        MelonPreferences_Category category,
        int minRange,
        int maxRange,
        Vector3 offsetHighest,
        Vector3 offset
    )
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

    internal Vector3 GetOffset(bool isHighestActive) => isHighestActive ? OffsetHighest : Offset;

    internal string GetRange() => RangeMs + "ms";

    private void CreateEntry(MelonPreferences_Category category)
    {
        _localRange = category.CreateEntry<float>(
            Name,
            MaxRange,
            description: $"In ms, has to be between {MinRange} and {MaxRange}",
            validator: new ValueRange<int>(MinRange, MaxRange)
        );
    }

    internal static JudgementRange CreateGreatRange(
        string name,
        string display,
        MelonPreferences_Category category,
        Vector3 highestOffset,
        Vector3 offset
    )
    {
        return new(name, display, category, 0, 80, highestOffset, offset);
    }

    internal static JudgementRange CreatePerfectRange(
        string name,
        string display,
        MelonPreferences_Category category,
        Vector3 highestOffset,
        Vector3 offset
    )
    {
        return new(name, display, category, 1, 50, highestOffset, offset);
    }
}
