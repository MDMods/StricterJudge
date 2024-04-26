using MelonLoader.Utils;
using StricterJudge.Models;
using StricterJudge.Properties;
using UnityEngine;

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
            Category,
            new Vector3(6f, -15f),
            new Vector3(3f, -5f)
        );

        PerfectLeftRange = new PerfectRangeClass("PerfectLeftRange",
            "LPerf",
            Category,
            new Vector3(3f, -10f),
            Vector3.zero);

        PerfectRightRange = new PerfectRangeClass("PerfectRightRange",
            "RPerf",
            Category,
            new Vector3(55f, 1f),
            new Vector3(18f, 4.5f)
        );

        GreatRightRange = new GreatRangeClass("GreatRightRange",
            "RGreat",
            Category,
            new Vector3(58f, -4f),
            new Vector3(21f, -0.5f));

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

    internal static List<RangeClass> Ranges { get; }

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