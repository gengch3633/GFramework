using Framework;
using System;

namespace GameFramework
{
    public interface IFreeCoinSystem : ISystem
    {
        void CollectCoins();
        void DecreaseColdDownTime();
        int GetCollectCoinCount(int level);
        int GetWaitTimeForNextCollect();
        bool IsCanCollectCoins();
    }

    public class FreeCoinSystem : AbstractSystem, IFreeCoinSystem
    {
        private FreeCoinRecord freeCoinRecord;

        protected override void OnInit()
        {
            freeCoinRecord = ReadInfoWithReturnNew<FreeCoinRecord>();
        }

        public bool IsCanCollectCoins()
        {
            return freeCoinRecord.IsCanCollectCoins();
        }

        public int GetCollectCoinCount(int level)
        {
            return 50 * level;
        }

        public void CollectCoins()
        {
            freeCoinRecord.SetCollectCoinTime();
            SaveInfo(freeCoinRecord);
        }

        public int GetWaitTimeForNextCollect()
        {
            return freeCoinRecord.GetWaitTimeForNextCollect();
        }

        public void DecreaseColdDownTime()
        {
            freeCoinRecord.DecreaseColdDownTime();
            SaveInfo(freeCoinRecord);
        }
    }

    public class FreeCoinRecord
    {
        public DateTime lastCollectFreeCoinTime = DateTime.MinValue;

        private int needWaitSeconds = 4 * 60 * 60;
        //private int needWaitSeconds = 60;
        public FreeCoinRecord(DateTime spinDate)
        {
            this.lastCollectFreeCoinTime = spinDate;
        }

        public FreeCoinRecord()
        {
        }

        public void SetCollectCoinTime()
        {
            this.lastCollectFreeCoinTime = DateTime.Now;
        }

        public bool IsCanCollectCoins()
        {
            var realWaitSeconds = (DateTime.Now - this.lastCollectFreeCoinTime).TotalSeconds;
            bool isCanSpin = realWaitSeconds >= needWaitSeconds;
            return isCanSpin;
        }
        
        public int GetWaitTimeForNextCollect()
        {
            int realWaitSeconds = (int)(DateTime.Now - this.lastCollectFreeCoinTime).TotalSeconds;
            
            return needWaitSeconds - realWaitSeconds;
        }

        public void DecreaseColdDownTime()
        {
            //if (this.lastCollectFreeCoinTime == DateTime.MinValue) this.lastCollectFreeCoinTime = DateTime.Now;
            this.lastCollectFreeCoinTime = lastCollectFreeCoinTime.AddMinutes(-10);
        }
    }
}
