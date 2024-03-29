﻿using StricterJudge.Models;

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
            $"{GreatLeftRange.GetDescription()}",
            $"{PerfectLeftRange.GetDescription()}",
            $"{PerfectRightRange.GetDescription()}",
            $"{GreatRightRange.GetDescription()}"
        ];

        MelonLogger.Msg(string.Join("\r\n", messages));
    }
}