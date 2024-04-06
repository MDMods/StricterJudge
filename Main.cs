using StricterJudge.Properties;

namespace StricterJudge;

public sealed partial class Main : MelonMod
{
    public override void OnInitializeMelon()
    {
        Load();
        LoggerInstance.Msg($"{MelonBuildInfo.ModName} has loaded correctly!");
    }
}