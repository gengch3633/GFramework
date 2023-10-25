namespace Framework
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanGetSystem
    {
        void Init();
    }

    public abstract class AbstractSystem : FileSaver, ISystem
    {
        private IArchitecture mArchitecture = null;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        void ISystem.Init()
        {
            OnInit();
            OnInitPartial();
            OnAfterInit();
        }

        protected virtual void OnInit() { }
        protected virtual void OnInitPartial() { }
        protected virtual void OnAfterInit() { }
    }
}