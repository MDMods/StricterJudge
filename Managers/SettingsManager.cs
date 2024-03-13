using MelonLoader;
using UnityEngine;
using Decimal = Il2CppSystem.Decimal;

namespace StricterJudge.Managers;

internal static class SettingsManager
{
    private const string SettingsPath = "UserData/StricterJudge.cfg";

    private static MelonPreferences_Entry<bool> _isEnabled;

    internal static bool IsEnabled
    {
        get => _isEnabled.Value;
        set => _isEnabled.Value = value;
    }

    internal static PerfectRangeClass PerfectLeftRange { get; private set; }
    internal static PerfectRangeClass PerfectRightRange { get; private set; }
    internal static GreatRangeClass GreatLeftRange { get; private set; }
    internal static GreatRangeClass GreatRightRange { get; private set; }

    internal static void Load()
    {
        var mainCategory = MelonPreferences.CreateCategory("StricterJudge");
        mainCategory.SetFilePath(SettingsPath, true, false);

        _isEnabled = mainCategory.CreateEntry(nameof(IsEnabled), true);

        GreatLeftRange = new GreatRangeClass("GreatLeftRange",
            "GLeft",
            mainCategory);

        PerfectLeftRange = new PerfectRangeClass("PerfectLeftRange",
            "PLeft",
            mainCategory);

        PerfectRightRange = new PerfectRangeClass("PerfectRightRange",
            "PRight",
            mainCategory);

        GreatRightRange = new GreatRangeClass("GreatRightRange",
            "GRight",
            mainCategory);

        string[] messages =
        [
            "StricterJudge range values:",
            $"{PerfectLeftRange.GetDescription()}",
            $"{PerfectRightRange.GetDescription()}",
            $"{GreatLeftRange.GetDescription()}",
            $"{GreatRightRange.GetDescription()}"
        ];

        foreach (var message in messages) Melon<Main>.Logger.Msg($"{message}");
    }

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
            LoadEntry(category);
        }

        private int MinRange { get; }
        private int MaxRange { get; }

        internal string Name { get; init; }
        internal string DisplayText { get; init; }

        private int LocalRange
        {
            get => (int)_localRange.Value;
            set => _localRange.Value = value;
        }

        private int RangeMs => LocalRange;
        internal Decimal RangeDec { get; private set; }

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

            if (originalRange == LocalRange) return;

            var warningMessage = $"Your selected range for {Name} is out of bounds.";
            warningMessage += $"The value has to be between {minRange} and {maxRange}.";
            Melon<Main>.Logger.Warning(warningMessage);
        }

        private void LoadEntry(MelonPreferences_Category category)
        {
            BaseRangeEntry(category, MinRange, MaxRange);
            InitEntryValue(MinRange, MaxRange);
        }

        internal string GetRange()
        {
            return RangeMs + "ms";
        }

        internal string GetDescription()
        {
            return $"{Name}: {ToString()}";
        }
    }

    internal class PerfectRangeClass : RangeClass
    {
        internal PerfectRangeClass(string name, string display, MelonPreferences_Category category
        ) : base(name, display, category, 1, 50)
        {
        }
    }

    internal class GreatRangeClass : RangeClass
    {
        internal GreatRangeClass(string name, string display, MelonPreferences_Category category
        ) : base(name, display, category, 0, 80)
        {
        }
    }
}