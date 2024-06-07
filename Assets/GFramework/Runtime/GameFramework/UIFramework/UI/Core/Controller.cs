using Framework;

namespace GameFramework
{
    public class Controller : IController
    {
        public ILanguageSystem LanguageSystem { get => this.GetSystem<ILanguageSystem>(); }
        public IUISystem UISystem { get => this.GetSystem<IUISystem>(); }
        public IDailyBonusSystem DailyBonusSystem { get => this.GetSystem<IDailyBonusSystem>(); }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }
}

