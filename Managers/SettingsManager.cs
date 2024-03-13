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

    internal abstract class RangeClass
    {
        private int MinRange { get; init; }
        private int MaxRange { get; init; }
        
        internal string Name { get; init; }
        internal string DisplayText { get; init; }
        
        private MelonPreferences_Entry<float> _localRange;
        private int LocalRange
        {
            get => (int)_localRange.Value;
            set => _localRange.Value = value;
        }
        
        internal int RangeMs => LocalRange;
        internal Decimal RangeDec { get; private set; }
        
        private void BaseRangeEntry(MelonPreferences_Category category, int minRange, int maxRange) =>
            _localRange = category.CreateEntry<float>(Name,
                maxRange,
                description: $"In ms, has to be between {minRange} and {maxRange}");

        private void InitEntryValue(int minRange, int maxRange)
        {
            int originalRange = LocalRange;
            LocalRange = Mathf.Clamp(LocalRange, minRange, maxRange);

            RangeDec = (Decimal)RangeMs / 1000;
            
            if (originalRange == LocalRange) return;
            
            string warningMessage = $"Your selected range for {Name} is out of bounds.";
            warningMessage += $"The value has to be between {minRange} and {maxRange}.";
            Melon<Main>.Logger.Warning(warningMessage);
        }

        protected void LoadEntry(MelonPreferences_Category category)
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

        protected RangeClass(string name, string display, MelonPreferences_Category category,
            int minRange, int maxRange)
        {
            Name = name;
            DisplayText = display;
            MinRange = minRange;
            MaxRange = maxRange;
            LoadEntry(category);
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

    internal static PerfectRangeClass PerfectLeftRange { get; set; }
    internal static PerfectRangeClass PerfectRightRange { get; set; }
    internal static GreatRangeClass GreatLeftRange { get; set; }
    internal static GreatRangeClass GreatRightRange { get; set; }

    internal static void Load()
    {
        MelonPreferences_Category mainCategory = MelonPreferences.CreateCategory("StricterJudge");
        mainCategory.SetFilePath(SettingsPath, true, false);

        _isEnabled = mainCategory.CreateEntry<bool>(nameof(IsEnabled), true);
        
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
        
        foreach(string message in messages) Melon<Main>.Logger.Msg($"{message}");
    }
}