using Framework;

namespace GameFramework
{
    public interface IStatisticsModel : IModel
    {
        public BindableProperty<int> FirstPlaceCount { get; }
        public BindableProperty<int> SecondPlaceCount { get; }
        public BindableProperty<int> ThirdPlaceCount { get; }
        public BindableProperty<int> ShootTheMoonCount { get; }
        public BindableProperty<int> RoundPlayedCount { get; }
        public BindableProperty<int> GamePlayedCount { get; }
    }

    public class StatisticsModel : AbstractModel, IStatisticsModel
    {
        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<StatisticsModel>();
            CopyBindableClass(this, userRecord);

            FirstPlaceCount.RegisterOnValueChanged(v => SaveInfo(this));
            SecondPlaceCount.RegisterOnValueChanged(v => SaveInfo(this));
            ThirdPlaceCount.RegisterOnValueChanged(v => SaveInfo(this));
            ShootTheMoonCount.RegisterOnValueChanged(v => SaveInfo(this));
            RoundPlayedCount.RegisterOnValueChanged(v => SaveInfo(this));
            GamePlayedCount.RegisterOnValueChanged(v => SaveInfo(this));
        }

        public BindableProperty<int> FirstPlaceCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> SecondPlaceCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> ThirdPlaceCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> ShootTheMoonCount { get; } = new BindableProperty<int>();

        public BindableProperty<int> RoundPlayedCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> RoundPlayedWinCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> GamePlayedCount { get; } = new BindableProperty<int>();
        public BindableProperty<int> GamePlayedWinCount { get; } = new BindableProperty<int>();
    }
}