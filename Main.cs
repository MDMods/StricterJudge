using MuseDashMirror;
using StricterJudge.Properties;

namespace StricterJudge;

public sealed partial class Main : MelonMod
{
    internal static void QueueReload(object sender, FileSystemEventArgs e)
    {
        if (!SceneInfo.IsGameScene)
        {
            Reload();
            return;
        }

        ReloadEvent = null;
        ReloadEvent += Reload;
    }

    private static void Reload()
    {
        Load();
        ReloadToggle();
        Melon<Main>.Logger.Msg($"{MelonBuildInfo.ModName} reloaded successfully!");
    }

    private static event Action ReloadEvent;

    public override void OnApplicationQuit()
    {
        DisableWatcherEvents();
        base.OnApplicationQuit();
    }

    public override void OnInitializeMelon()
    {
        Load();
        LoggerInstance.Msg($"{MelonBuildInfo.ModName} has loaded correctly!");
    }

    public override void OnLateInitializeMelon()
    {
        EnableWatcherEvents();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        // Reload if needed outside of GameMain
        if (SceneInfo.IsGameScene) return;
        ReloadEvent?.Invoke();
        ReloadEvent = null;
    }
}