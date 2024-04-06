using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Object = Il2CppSystem.Object;

namespace StricterJudge.Patches;

[HarmonyPatch(typeof(PnlVictory), nameof(PnlVictory.OnVictory), typeof(Object), typeof(Object),
    typeof(Il2CppReferenceArray<Object>))]
internal static class VictoryPatch
{
    internal static void Postfix(PnlVictory __instance)
    {
        if (!IsEnabled) return;

        try
        {
            var parent = __instance.m_CurControls.highScoreTxt.transform.parent;
            var baseGo = parent.gameObject.FastInstantiate();

            CreateTextObjects(baseGo, parent);

            baseGo.Destroy();
        }
        catch (Exception e)
        {
            Melon<Main>.Logger.Error(e.ToString());
        }
    }
}