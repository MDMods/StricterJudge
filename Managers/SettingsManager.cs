using MelonLoader.Utils;
using StricterJudge.Models;
using StricterJudge.Properties;

namespace StricterJudge.Managers;

using static MelonEnvironment;

internal static class SettingsManager
{
    private const string SettingsFileName = $"{MelonBuildInfo.ModName}.cfg";

    private const string SettingsPath = $"UserData/{SettingsFileName}";

    private static readonly MelonPreferences_Entry<bool> IsEnabledEntry;

    private static readonly MelonPreferences_Category Category;

    private static readonly FileSystemWatcher Watcher = new(UserDataDirectory);

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

        // Create file at runtime if it doesn't exists
        var absolutePath = Path.Join(UserDataDirectory, SettingsFileName);
        if (!File.Exists(absolutePath)) MelonPreferences.Save();

        // Initialize file watcher
        Watcher.NotifyFilter = NotifyFilters.LastWrite
                               | NotifyFilters.Size;

        Watcher.Filter = SettingsFileName;

        Watcher.Changed += Main.QueueReload;
    }

    internal static bool IsEnabled
    {
        get => IsEnabledEntry.Value;
        set
        {
            IsEnabledEntry.Value = value;
            IsEnabledSet();
        }
    }

    internal static GreatRangeClass GreatLeftRange { get; }

    internal static PerfectRangeClass PerfectLeftRange { get; }

    internal static PerfectRangeClass PerfectRightRange { get; }

    internal static GreatRangeClass GreatRightRange { get; }

    private static List<RangeClass> Ranges { get; }

    internal static void DisableWatcherEvents()
    {
        Watcher.EnableRaisingEvents = false;
    }

    internal static void EnableWatcherEvents()
    {
        Watcher.EnableRaisingEvents = true;
    }

    internal static void Load()
    {
        Category.LoadFromFile(false);

        Ranges.ForEach(range => range.InitEntryValue());

        List<string> messages =
        [
            "StricterJudge range values:"
        ];

        messages.AddRange(Ranges.Select(val => val.GetDescription()));

        MelonLogger.Msg(string.Join("\r\n", messages));
    }

    private static void IsEnabledSet()
    {
        if (Watcher is null) return;

        Watcher.EnableRaisingEvents = false;

        Category.SaveToFile(false);

        EnableWatcherEvents();
    }
}