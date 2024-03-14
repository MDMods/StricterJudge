using System.Reflection;
using HarmonyLib;
using Il2Cpp;
using MuseDashMirror.Extensions;
using Object = UnityEngine.Object;

namespace StricterJudge.Patches;

[HarmonyPatch]
internal static class VictoryPatch
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(PnlVictory).GetMethods().Where(m => m.Name.Equals(nameof(PnlVictory.OnVictory)));
    }

    internal static void Postfix(PnlVictory __instance)
    {
        if (!IsEnabled)
        {
            return;
        }

        var parent = __instance.m_CurControls.highScoreTxt.transform.parent;
        var baseGo = Object.Instantiate(parent.gameObject);

        CreateTextObjects(baseGo, parent);

        baseGo.Destroy();
    }
}