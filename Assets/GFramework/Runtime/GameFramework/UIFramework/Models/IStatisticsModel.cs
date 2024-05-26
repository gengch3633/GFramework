using Framework;

namespace GameFramework
{
    public interface IStatisticsModel : IModel
    {
        public BindableProperty<int> FirstPlaceCount { get; }
    }

    public partial class StatisticsModel : AbstractModel, IStatisticsModel
    {
        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<StatisticsModel>();
            CopyBindableClass(this, userRecord);

            FirstPlaceCount.RegisterOnValueChanged(v => SaveInfo(this));
        }

        public BindableProperty<int> FirstPlaceCount { get; } = new BindableProperty<int>();
    }
}