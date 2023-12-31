using Framework;
using System.Collections.Generic;

namespace GameFramework
{
    public partial interface IUserModel : IModel
    {
        BindableProperty<int> Level { get; }
        BindableProperty<int> Coins { get; }
        BindableProperty<int> Score { get; }
        BindableProperty<string> Name { get; }

        bool IsCoinEnough(int coins);

        bool IsCanLevelUp();
        float GetCurrentLevelProgress();

        LevelInfo GetCurrentLevelInfo();
        int GetCurrentLevelEndScore();

        int GetCurrentLevelStartScore();

        List<LevelInfo> GetLevelInfos();
    }

    public partial class UserModel : AbstractModel, IUserModel
    {
        private List<LevelInfo> levelInfos;

        public BindableProperty<int> Level { get; } = new BindableProperty<int>() { Value = 1 };
        public BindableProperty<int> Coins { get; } = new BindableProperty<int>() { Value = 200 };
        public BindableProperty<string> Name { get; } = new BindableProperty<string>() { Value = "YOU" };
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<UserModel>();
            CopyBindableClass(this, userRecord);
            Level.RegisterOnValueChanged(v => SaveInfo(this));
            Coins.RegisterOnValueChanged(v => SaveInfo(this));
            Score.RegisterOnValueChanged(v => SaveInfo(this));
            Name.RegisterOnValueChanged(v => SaveInfo(this));

            levelInfos = GameUtils.GetConfigInfos<LevelInfo>();
        }

        public List<LevelInfo> GetLevelInfos()
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

        public LevelInfo GetCurrentLevelInfo()
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