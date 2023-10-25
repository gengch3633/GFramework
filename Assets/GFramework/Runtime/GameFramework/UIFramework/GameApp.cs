using Framework;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 程序注册中心
    /// 在第一个界面引用时初始化
    /// </summary>
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            this.RegisterSystem<IDebugSystem>(new DebugSystem());
            this.RegisterSystem<INotificationSystem>(new NotificationSystem());

            this.RegisterSystem<IResourceSystem>(new ResourceSystem());
            this.RegisterSystem<IAudioSystem>(new AudioSystem());

            this.RegisterModel<IGameModel>(new GameModel());
           
            this.RegisterSystem<ILanguageSystem>(new LanguageSystem());
            this.RegisterSystem<IUISystem>(new UISystem());
            this.RegisterSystem<IRateSystem>(new RateSystem());
            this.RegisterSystem<IFreeCoinSystem>(new FreeCoinSystem());
            
            this.RegisterSystem<IAdsSystem>(new AdsSystem());

            this.RegisterSystem<IFirebaseSystem>(new FirebaseSystem());
            this.RegisterSystem<ISdkSystem>(new SdkSystem());
            this.RegisterSystem<IDailyBonusSystem>(new DailyBonusSystem());
            this.RegisterSystem<ISpinSystem>(new SpinSystem());

            this.RegisterModel<ISettingModel>(new SettingModel());
            this.RegisterModel<IUserModel>(new UserModel());


            this.RegisterSystem<IDailyTaskSystem>(new DailyTaskSystem());
            this.RegisterModel<IStatisticsModel>(new StatisticsModel());
        }
    }
}
