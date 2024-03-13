using HarmonyLib;
using Il2CppFormulaBase;
using Il2CppGameLogic;
using StricterJudge.Managers;

namespace StricterJudge.Patches;

using static ModManager;
using static SettingsManager;

[HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GetMusicDataFromStageInfo))]
internal static class JudgePatch
{
    private static void Postfix(ref Il2CppSystem.Collections.Generic.List<MusicData> __result)
    {
        if (!IsEnabled) return;

        for (var i = 0; i < __result.Count; i++)
        {
            MusicData musicData = __result[i];

            if (!UpdateNoteJudge(ref musicData)) continue;
            __result[i] = musicData;
        }
    }
}