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
        float GetLevelProgress();

        LevelInfo GetLevelInfo();
        int GetLevelEndScore();

        int GetLevelStartScore();

        List<LevelInfo> GetLevelInfos();
    }

    public partial class UserModel : AbstractModel, IUserModel
    {
        protected IResourceSystem resourceSystem;
        private List<LevelInfo> levelInfos;
        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<UserModel>();
            CopyBindableClass(this, userRecord);
            Level.RegisterOnValueChanged(v => SaveInfo(this));
            Coins.RegisterOnValueChanged(v => SaveInfo(this));
            Score.RegisterOnValueChanged(v => SaveInfo(this));
            Name.RegisterOnValueChanged(v => SaveInfo(this));

            resourceSystem = GameApp.Interface.GetSystem<IResourceSystem>();
            levelInfos = GameUtils.GetConfigInfos<LevelInfo>();
        }

        public List<LevelInfo> GetLevelInfos()
        {
            return levelInfos;
        }

        public LevelInfo GetLevelInfo()
        {
            var levelInfo = levelInfos.Find(item => item.level == Level.Value);
            return levelInfo;
        }

        public bool IsCoinEnough(int coins)
        {
            var ret = Coins.Value >= coins;
            return ret;
        }

        public float GetLevelProgress()
        {
            var levelStartScore = GetLevelStartScore();
            var levelEndScore = GetLevelEndScore();
            var levelCollectScore = Score.Value - levelStartScore;
            var levelNeedScore = levelEndScore - levelStartScore;
            var toProgress = 1.0f * levelCollectScore / levelNeedScore;
            return toProgress;
        }

        public int GetLevelStartScore()
        {
            var levelId = Level.Value - 1;
            int targetScore = GetLevelFullScore(levelId);
            return targetScore;
        }
        public int GetLevelEndScore()
        {
            var levelId = Level.Value;
            int targetScore = GetLevelFullScore(levelId);
            return targetScore;
        }

        public bool IsCanLevelUp()
        {
            return Score.Value >= GetLevelEndScore();
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

        public BindableProperty<int> Level { get; } = new BindableProperty<int>() { Value = 1 };
        public BindableProperty<int> Coins { get; } = new BindableProperty<int>() { Value = 200 };
        public BindableProperty<string> Name { get; } = new BindableProperty<string>() { Value = "YOU" };
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
    }
}