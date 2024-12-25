using Framework;
using System.Collections.Generic;

namespace GameFramework
{
    public partial interface IUserModel : IModel
    {
        BindableProperty<float> BannerHeight { get; }
        BindableProperty<int> Level { get; }
        BindableProperty<int> Coins { get; }
        BindableProperty<int> Score { get; }
        BindableProperty<int> Diamonds { get; }
        BindableProperty<string> Name { get; }
        BindableProperty<int> OpenPopupAnimWaitFrameCount { get; }

        bool IsCoinEnough(int coins);

        bool IsCanLevelUp();
        float GetCurrentLevelProgress();

        LevelUpInfo GetCurrentLevelInfo();
        int GetCurrentLevelEndScore();

        int GetCurrentLevelStartScore();

        List<LevelUpInfo> GetLevelInfos();
    }

    public partial class UserModel : AbstractModel, IUserModel
    {
        private List<LevelUpInfo> levelInfos;

        public BindableProperty<float> BannerHeight { get; } = new BindableProperty<float>() { Value = 0 };
        public BindableProperty<int> Level { get; } = new BindableProperty<int>() { Value = 1 };
        public BindableProperty<int> Coins { get; } = new BindableProperty<int>() { Value = 200 };
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
        public BindableProperty<int> Diamonds { get; } = new BindableProperty<int>() { Value = 200 };
        public BindableProperty<string> Name { get; } = new BindableProperty<string>() { Value = "YOU" };
        public BindableProperty<int> OpenPopupAnimWaitFrameCount { get; } = new BindableProperty<int>();
        
        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<UserModel>();
            CopyBindableClass(this, userRecord, ()=> SaveInfo(this));

            levelInfos = GameUtils.GetConfigInfos<LevelUpInfo>();
        }

        public List<LevelUpInfo> GetLevelInfos()
        {
            return levelInfos;
        }

        public bool IsCoinEnough(int coins)
        {
            var ret = Coins.Value >= coins;
            return ret;
        }

        public bool IsCanLevelUp()
        {
            return Score.Value >= GetCurrentLevelEndScore();
        }

        public LevelUpInfo GetCurrentLevelInfo()
        {
            var levelInfo = levelInfos.Find(item => item.level == Level.Value);
            return levelInfo;
        }

        public float GetCurrentLevelProgress()
        {
            var levelStartScore = GetCurrentLevelStartScore();
            var levelEndScore = GetCurrentLevelEndScore();
            var levelCollectScore = Score.Value - levelStartScore;
            var levelNeedScore = levelEndScore - levelStartScore;
            var toProgress = 1.0f * levelCollectScore / levelNeedScore;
            return toProgress;
        }

        public int GetCurrentLevelStartScore()
        {
            var levelId = Level.Value - 1;
            int targetScore = GetLevelFullScore(levelId);
            return targetScore;
        }
        public int GetCurrentLevelEndScore()
        {
            var levelId = Level.Value;
            int targetScore = GetLevelFullScore(levelId);
            return targetScore;
        }

        private int GetLevelFullScore(int levelId)
        {
            var targetScore = 0;
            for (int i = 0; i < levelId; i++)
            {
                var score = levelInfos[i].levelUpExp;
                targetScore += score;
            }

            return targetScore;
        }
    }
}