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

        GreatLeftRange = JudgementRange.CreateGreatRange(
            "GreatLeftRange",
            "LGreat",
            Category,
            new Vector3(6f, -15f),
            new Vector3(3f, -5f)
        );

        PerfectLeftRange = JudgementRange.CreatePerfectRange(
            "PerfectLeftRange",
            "LPerf",
            Category,
            new Vector3(3f, -10f),
            Vector3.zero
        );

        PerfectRightRange = JudgementRange.CreatePerfectRange(
            "PerfectRightRange",
            "RPerf",
            Category,
            new Vector3(55f, 1f),
            new Vector3(18f, 4.5f)
        );

        GreatRightRange = JudgementRange.CreatePerfectRange(
            "GreatRightRange",
            "RGreat",
            Category,
            new Vector3(58f, -4f),
            new Vector3(21f, -0.5f)
        );

        Ranges = [GreatLeftRange, PerfectLeftRange, PerfectRightRange, GreatRightRange];

        // Create file at runtime if it doesn't exists
        var absolutePath = Path.Join(UserDataDirectory, SettingsFileName);
        if (!File.Exists(absolutePath))
            MelonPreferences.Save();

        // Initialize file watcher
        Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

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

    internal static JudgementRange GreatLeftRange { get; }

    internal static JudgementRange PerfectLeftRange { get; }

    internal static JudgementRange PerfectRightRange { get; }

    internal static JudgementRange GreatRightRange { get; }

    internal static List<JudgementRange> Ranges { get; }

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

        List<string> messages = ["StricterJudge range values:"];

        messages.AddRange(Ranges.Select(val => val.GetDescription()));

        MelonLogger.Msg(string.Join("\r\n", messages));
    }

    private static void IsEnabledSet()
    {
        if (Watcher is null)
            return;

        Watcher.EnableRaisingEvents = false;

        Category.SaveToFile(false);

        EnableWatcherEvents();
    }
}
