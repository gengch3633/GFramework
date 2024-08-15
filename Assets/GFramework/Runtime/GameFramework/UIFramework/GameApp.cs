using Framework;
using UnityEngine;

namespace GameFramework
{
    public partial class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            InitBaseModels();
            InitBaseSystems();
        }

        private void InitBaseSystems()
        {
            this.RegisterSystem<IResourceSystem>(new ResourceSystem());
            this.RegisterSystem<IAudioSystem>(new AudioSystem());
            this.RegisterSystem<ILanguageSystem>(new LanguageSystem());
            this.RegisterSystem<IUISystem>(new UISystem());
            this.RegisterSystem<IRateSystem>(new RateSystem());
            this.RegisterSystem<IAdsSystem>(new AdsSystem());

            this.RegisterSystem<ITutorialSystem>(new TutorialSystem());
            this.RegisterSystem<IFreeCoinSystem>(new FreeCoinSystem());
            this.RegisterSystem<ISpinSystem>(new SpinSystem());
            this.RegisterSystem<IDailyBonusSystem>(new DailyBonusSystem());
            this.RegisterSystem<IDailyTaskSystem>(new DailyTaskSystem());
        }

        private void InitBaseModels()
        {
            this.RegisterModel<IUserModel>(new UserModel());
            this.RegisterModel<IDebugModel>(new DebugModel());
        }
    }
}
