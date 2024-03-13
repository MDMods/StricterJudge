using System.Reflection;
using HarmonyLib;
using Il2Cpp;
using MuseDashMirror.Extensions;
using StricterJudge.Managers;
using Object = UnityEngine.Object;

namespace StricterJudge.Patches;

using static ModManager;
using static SettingsManager;

[HarmonyPatch]
internal class VictoryPatch
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(PnlVictory).GetMethods().Where(m => m.Name.Equals(nameof(PnlVictory.OnVictory)));
    }

    private static void Postfix(PnlVictory __instance)
    {
        if (!IsEnabled) return;

        var parent = __instance.m_CurControls.highScoreTxt.transform.parent;
        var baseGo = Object.Instantiate(parent.gameObject);

        CreateTextObjects(baseGo, parent);

        baseGo.Destroy();
    }
}