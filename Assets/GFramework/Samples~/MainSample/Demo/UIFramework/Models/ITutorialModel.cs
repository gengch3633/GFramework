using Framework;
using System;

namespace GameFramework
{
    public partial interface ITutorialModel : IModel
    {
       
    }

    public partial class TutorialModel : AbstractModel
    {
        private TutorialModel tutorialRecord;

        protected override void OnInit()
        {
            base.OnInit();
            tutorialRecord = ReadInfoWithReturnNew<TutorialModel>();
            CopyBindableClass(this, tutorialRecord);
        }
    }
}