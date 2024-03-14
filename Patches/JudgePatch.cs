using HarmonyLib;
using Il2CppFormulaBase;
using Il2CppGameLogic;

namespace StricterJudge.Patches;

[HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GetMusicDataFromStageInfo))]
internal static class JudgePatch
{
    internal static void Postfix(Il2CppSystem.Collections.Generic.List<MusicData> __result)
    {
        if (!IsEnabled)
        {
            return;
        }

        for (var i = 0; i < __result.Count; i++)
        {
            MusicData musicData = __result[i];

            if (!UpdateNoteJudge(ref musicData))
            {
                continue;
            }

            __result[i] = musicData;
        }
    }
}