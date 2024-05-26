using Framework;
using UnityEngine;

namespace GameFramework
{
    public partial class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            var ganeAppExt = new GameAppExt();
            InitBaseModels();
            ganeAppExt.InitModels();
            InitBaseSystems();
            ganeAppExt.InitSystems();
        }

        private void InitBaseSystems()
        {
            this.RegisterSystem<IResourceSystem>(new ResourceSystem());
            this.RegisterSystem<IAudioSystem>(new AudioSystem());
            this.RegisterSystem<ILanguageSystem>(new LanguageSystem());
            this.RegisterSystem<IUISystem>(new UISystem());
            this.RegisterSystem<IRateSystem>(new RateSystem());
            this.RegisterSystem<IAdsSystem>(new AdsSystem());

            this.RegisterSystem<IFreeCoinSystem>(new FreeCoinSystem());
            this.RegisterSystem<ISpinSystem>(new SpinSystem());
            this.RegisterSystem<IDailyBonusSystem>(new DailyBonusSystem());
            this.RegisterSystem<IDailyTaskSystem>(new DailyTaskSystem());
        }

        private void InitBaseModels()
        {
            this.RegisterModel<IGameModel>(new GameModel());
            this.RegisterModel<ISettingModel>(new SettingModel());
            this.RegisterModel<IUserModel>(new UserModel());
            this.RegisterModel<IStatisticsModel>(new StatisticsModel());
            this.RegisterModel<IDebugModel>(new DebugModel());
        }
    }
}
