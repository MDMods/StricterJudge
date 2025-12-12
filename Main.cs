using Il2CppAssets.Scripts.UI.Tips;
using StricterJudge.Managers;
using StricterJudge.Properties;
using UnityEngine;

namespace StricterJudge;

public class Main : MelonMod
{
    private static event Action ReloadEvent;

    public static bool IsGameScene { get; private set; } = false;

    private static bool IsControlPressed =>
        Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

    private readonly List<AbstractMessageBox> abstractMessageBoxes = [];

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
        IsGameScene = sceneName?.Equals("GameMain") ?? false;
        // Reload if needed outside of GameMain
        if (IsGameScene)
        {
            return;
        }

        ReloadEvent?.Invoke();
        ReloadEvent = null;
    }

    public override void OnUpdate()
    {
        if (IsGameScene)
        {
            return;
        }

        if (IsControlPressed && Input.GetKeyDown(SettingsManager.Key))
        {
            var previousState = IsEnabled;
            IsEnabled = !IsEnabled;

            var message =
                $"{MelonBuildInfo.ModName} - toggling enabled state from {previousState} to {IsEnabled}.";

            abstractMessageBoxes.ForEach(box => box?.Close());
            abstractMessageBoxes.Clear();

            var box = PnlTipsManager.instance?.GetMessageBox("PnlSpecialsBmsAsk");
            if (box != null)
            {
                box.Show("", message);
                abstractMessageBoxes.Add(box);
            }
            Melon<Main>.Logger.Msg(message);
        }
    }

    internal static void QueueReload(object sender, FileSystemEventArgs e)
    {
        if (!IsGameScene)
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
        Melon<Main>.Logger.Msg($"{MelonBuildInfo.ModName} reloaded successfully!");
    }
}
