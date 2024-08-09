using UnityEngine;

namespace Framework
{
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent, IFileSaver
    {
        void Init();
    }


    public abstract class AbstractModel :  FileSaver, IModel
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

        void IModel.Init()
        {
            OnInit();
            OnInitPartial();
        }

        protected virtual void OnInit() { }
        protected virtual void OnInitPartial() { }
    }
}