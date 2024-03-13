using HarmonyLib;
using Il2Cpp;
using MuseDashMirror.Extensions;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StricterJudge.Patches;

using static Managers.ModManager;
using static Managers.SettingsManager;

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

        Transform parent = __instance.m_CurControls.highScoreTxt.transform.parent;
        GameObject baseGo = Object.Instantiate(parent.gameObject);
        
        CreateTextObjects(baseGo, parent);

        baseGo.Destroy();
    }
}