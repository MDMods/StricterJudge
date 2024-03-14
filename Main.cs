using MelonLoader;

namespace StricterJudge;

public partial class Main : MelonMod
{
    public override void OnInitializeMelon()
    {
        Load();
        LoggerInstance.Msg("StricterJudge has loaded correctly!");
    }
}