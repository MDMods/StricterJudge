using MelonLoader;
using StricterJudge.Models;

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
            "LGreat",
            mainCategory);

        PerfectLeftRange = new PerfectRangeClass("PerfectLeftRange",
            "LPerf",
            mainCategory);

        PerfectRightRange = new PerfectRangeClass("PerfectRightRange",
            "RPerf",
            mainCategory);

        GreatRightRange = new GreatRangeClass("GreatRightRange",
            "RGreat",
            mainCategory);

        string[] messages =
        [
            "StricterJudge range values:",
            $"{PerfectLeftRange.GetDescription()}",
            $"{PerfectRightRange.GetDescription()}",
            $"{GreatLeftRange.GetDescription()}",
            $"{GreatRightRange.GetDescription()}"
        ];

        MelonLogger.Msg(string.Join("\r\n", messages));
    }

    internal sealed class PerfectRangeClass : RangeClass
    {
        internal PerfectRangeClass(string name, string display, MelonPreferences_Category category)
            : base(name, display, category, 1, 50)
        {
        }
    }

    internal sealed class GreatRangeClass : RangeClass
    {
        internal GreatRangeClass(string name, string display, MelonPreferences_Category category)
            : base(name, display, category, 0, 80)
        {
        }
    }
}