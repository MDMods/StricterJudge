using MelonLoader;
using StricterJudge.Managers;

namespace StricterJudge
{
    public partial class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            SettingsManager.Load();
            LoggerInstance.Msg("StricterJudge has loaded correctly!");
        }
    }
}