using Framework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface ISpinSystem : ISystem
    {
        //获取剩余的可抽奖次数
        int GetRemainedSpinCount();
        //是否处于可抽奖状态
        bool IsCanSpin();
        //抽奖
        void Spin();
        //获奖加成倍数
        int GetBonusTimes();
        //每天的总抽奖次数
        int GetAllSpinCount();
        //获得剩余冷却时间
        int GetColdDownSeconds();
        //跳过冷却时间
        void SkipColdDown();
        bool IsTodaySpined();
        void SetTodaySpined();
    }

    public class SpinSystem: AbstractSystem, ISpinSystem
    {
        private static readonly string Key = "SPIN_SYSTEM";
        private SpinRecord spinRecord;

        protected override void OnInit()
        {
            spinRecord = new SpinRecord(DateTime.Today);
            if (PlayerPrefs.HasKey(Key))
            {
                SpinRecord savedSpinRecord = JsonConvert.DeserializeObject<SpinRecord>(PlayerPrefs.GetString(Key));
                TimeSpan timeSpan = DateTime.Today - savedSpinRecord.spinDate;
                int spinDays = timeSpan.Days;
                spinRecord = spinDays == 0 ? savedSpinRecord: spinRecord;
            }
        }

        public bool IsTodaySpined()
        {
            return spinRecord.isTodaySpined;
        }

        public void SetTodaySpined()
        {
            spinRecord.isTodaySpined = true;
            SaveSpinRecord();
        }

        private void SaveSpinRecord()
        {
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(spinRecord));
        }

        public int GetRemainedSpinCount()
        {
            return spinRecord.remainedSpinCount;
        }

        public bool IsCanSpin()
        {
            return spinRecord.IsCanSpin();
        }

        public void Spin()
        {
            spinRecord.Spin();
            SaveSpinRecord();
        }

        public int GetBonusTimes()
        {
            return spinRecord.GetBonusTimes();
        }

        public int GetAllSpinCount()
        {
            return spinRecord.allSpinCount;
        }

        public int GetColdDownSeconds()
        {
            return spinRecord.GetColdSpinSeconds();
        }

        public void SkipColdDown()
        {
            spinRecord.SkipColdDown();
            SaveSpinRecord();
        }
    }

    public class SpinRecord
    {
        public DateTime spinDate;
        public DateTime lastSpinTime;
        public bool isTodaySpined = false;

        public int remainedSpinCount = 3;
        public int allSpinCount = 3;

        private const int minuteSeconds = 60;
        private int[] waitSeconds = new int[] { 0, 3 * minuteSeconds, 5 * minuteSeconds };
        public SpinRecord(DateTime spinDate)
        {
            this.spinDate = spinDate;
            this.lastSpinTime = spinDate;
        }

        public void Spin()
        {
            this.remainedSpinCount -= 1;
            this.lastSpinTime = DateTime.Now;
        }

        public bool IsCanSpin()
        {
            int waitIndex = this.allSpinCount - remainedSpinCount;
            long realWaitSeconds = (int)(DateTime.Now - this.lastSpinTime).TotalSeconds;

            long waitSecond = waitIndex < waitSeconds.Length ? waitSeconds[waitIndex] : long.MaxValue;
            bool isCanSpin = realWaitSeconds >= waitSecond;

            return isCanSpin;
        }
        
        public int GetColdSpinSeconds()
        {
            int waitIndex = this.allSpinCount - remainedSpinCount;
            waitIndex = waitIndex < waitSeconds.Length ? waitIndex : 0;

            int realWaitSeconds = (int)(DateTime.Now - this.lastSpinTime).TotalSeconds;
            int needWaitSeconds = waitSeconds[waitIndex];
            return needWaitSeconds - realWaitSeconds;
        }

        public int GetBonusTimes()
        {
            int times = this.allSpinCount - this.remainedSpinCount + 1;
            times = times <= this.allSpinCount ? times : allSpinCount;
            return times;
        }

        public void SkipColdDown()
        {
            this.lastSpinTime = DateTime.Now.AddMinutes(-20);
        }
    }
}
