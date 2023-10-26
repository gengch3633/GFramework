using Framework;
using UnityEngine;

namespace GameFramework
{
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            InitBaseSystems();
            InitSystemsWithThirdParty();
            InitModels();
        }

        private void InitSystemsWithThirdParty()
        {
            this.RegisterSystem<INotificationSystem>(new NotificationSystem());
            this.RegisterSystem<IAdsSystem>(new AdsSystem());
            this.RegisterSystem<IFirebaseSystem>(new FirebaseSystem());
            this.RegisterSystem<ISdkSystem>(new SdkSystem());
        }

        private void InitBaseSystems()
        {
            this.RegisterSystem<IDebugSystem>(new DebugSystem());
            this.RegisterSystem<IResourceSystem>(new ResourceSystem());
            this.RegisterSystem<IAudioSystem>(new AudioSystem());
            this.RegisterSystem<ILanguageSystem>(new LanguageSystem());
            this.RegisterSystem<IUISystem>(new UISystem());
            this.RegisterSystem<IRateSystem>(new RateSystem());
            this.RegisterSystem<IFreeCoinSystem>(new FreeCoinSystem());
            this.RegisterSystem<ISpinSystem>(new SpinSystem());
            this.RegisterSystem<IDailyBonusSystem>(new DailyBonusSystem());
            this.RegisterSystem<IDailyTaskSystem>(new DailyTaskSystem());
        }

        private void InitModels()
        {
            this.RegisterModel<IGameModel>(new GameModel());
            this.RegisterModel<ISettingModel>(new SettingModel());
            this.RegisterModel<IUserModel>(new UserModel());
            this.RegisterModel<IStatisticsModel>(new StatisticsModel());
        }
    }
}
