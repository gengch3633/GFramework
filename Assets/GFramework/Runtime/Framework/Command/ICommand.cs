namespace Framework
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanSendCommand
    {
        void Excute();
    }

    public abstract class AbstractCommand : ICommand
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

        void ICommand.Excute()
        {
            OnExcute();
        }

        protected abstract void OnExcute();
    }
}