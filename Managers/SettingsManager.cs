using MelonLoader.Utils;
using StricterJudge.Models;
using StricterJudge.Properties;

namespace StricterJudge.Managers;

internal static class SettingsManager
{
    private const string SettingsFileName = $"{MelonBuildInfo.ModName}.cfg";
    private const string SettingsPath = $"UserData/{SettingsFileName}";

    private static readonly MelonPreferences_Entry<bool> IsEnabledEntry;

    private static readonly MelonPreferences_Category Category;

    static SettingsManager()
    {
        Category = MelonPreferences.CreateCategory("StricterJudge");
        Category.SetFilePath(SettingsPath, false, false);

        IsEnabledEntry = Category.CreateEntry(nameof(IsEnabled), true);

        GreatLeftRange = new GreatRangeClass("GreatLeftRange",
            "LGreat",
            Category);

        PerfectLeftRange = new PerfectRangeClass("PerfectLeftRange",
            "LPerf",
            Category);

        PerfectRightRange = new PerfectRangeClass("PerfectRightRange",
            "RPerf",
            Category);

        GreatRightRange = new GreatRangeClass("GreatRightRange",
            "RGreat",
            Category);

        Ranges =
        [
            GreatLeftRange,
            PerfectLeftRange,
            PerfectRightRange,
            GreatRightRange
        ];
    }

    internal static bool IsEnabled
    {
        get => IsEnabledEntry.Value;
        set => IsEnabledEntry.Value = value;
    }

    internal static GreatRangeClass GreatLeftRange { get; }

    internal static PerfectRangeClass PerfectLeftRange { get; }

    internal static PerfectRangeClass PerfectRightRange { get; }

    internal static GreatRangeClass GreatRightRange { get; }

    private static List<RangeClass> Ranges { get; }

    internal static void Load()
    {
        Category.LoadFromFile(false);

        Ranges.ForEach(range => range.InitEntryValue());
        
        string[] messages =
        [
            "StricterJudge range values:",
            $"{GreatLeftRange.GetDescription()}",
            $"{PerfectLeftRange.GetDescription()}",
            $"{PerfectRightRange.GetDescription()}",
            $"{GreatRightRange.GetDescription()}"
        ];

        MelonLogger.Msg(string.Join("\r\n", messages));
    }
}