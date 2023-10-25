using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface IDailyBonusSystem : ISystem
    {
        //获取某日奖励数量
        int GetDailyBonusRewardCount(int dayNum);
        //获得某日奖励
        void ClaimDailyBonus(int dayNum);
        //判断某日是否已领取
        bool IsDailyBonusClaimed(int dayNum);
        //获得可领取奖励的当天Num
        int GetDailyBonusDayNum();
        //用于测试
        void IncreaseLoginDays();

        DailyBonus GetDailyBonus(int dayNum);
    }

    /// <summary>
    /// 每日奖励
    /// </summary>
    public class DailyBonusSystem : AbstractSystem, IDailyBonusSystem
    {
        private DailyBonusGroup dailyBonusGroup;
        private DailyBonusGroup initBonusGroup;
        private int dayLoopNum = 5;
        private static readonly string Key = "DAILY_BONUS_SYSTEM";

        protected override void OnInit()
        {
            var resourceSystem = this.GetSystem<IResourceSystem>();
            var dailyBonusList = GameUtils.GetConfigInfos<DailyBonus>();
            initBonusGroup = new DailyBonusGroup { 
                lastDailyBonusTime = DateTime.Today, 
                dailyBonusList = dailyBonusList 
            };

            dailyBonusGroup = initBonusGroup;
            UpdateTasks();
        }

        private void UpdateTasks()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                string dailyTaskInfoString = PlayerPrefs.GetString(Key);
                dailyBonusGroup = JsonConvert.DeserializeObject<DailyBonusGroup>(dailyTaskInfoString);
                TimeSpan timeSpan = DateTime.Today - dailyBonusGroup.lastDailyBonusTime;
                int spinDays = timeSpan.Days;

                if (spinDays != 0)
                    IncreaseLoginDays();
            }
        }

        private void SaveInfo()
        {
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(dailyBonusGroup));
        }

        public void ClaimDailyBonus(int dayNum)
        {
            dailyBonusGroup.ClaimBonus(dayNum);
            SaveInfo();
        }

        public int GetDailyBonusRewardCount(int dayNum)
        {
            return dailyBonusGroup.GetDailyBonusRewardCount(dayNum);
        }

        public bool IsDailyBonusClaimed(int dayNum)
        {
            return dailyBonusGroup.IsDailyBonusClaimed(dayNum);
        }

        public int GetDailyBonusDayNum()
        {
            return dailyBonusGroup.loginDays % dayLoopNum;
        }

        public void IncreaseLoginDays()
        {
            dailyBonusGroup.IncreaseLoginDays(dayLoopNum);
            SaveInfo();
        }

        public DailyBonus GetDailyBonus(int dayNum)
        {
            var ret = dailyBonusGroup.dailyBonusList.Find(item => item.dayNum == dayNum);
            return ret;
        }
    }

    public class DailyBonusGroup
    {
        public int loginDays = 0;
        public DateTime lastDailyBonusTime;
        public List<DailyBonus> dailyBonusList;

        public void IncreaseLoginDays(int loopDays)
        {
            loginDays += 1;
            lastDailyBonusTime = DateTime.Today;
            if (loginDays % loopDays == 0)
                dailyBonusList.ForEach(item => item.isClaimed = false);
        }

        public void ClaimBonus(int dayNum)
        {
            DailyBonus dailyBonus = dailyBonusList.Find(item => item.dayNum == dayNum);
            dailyBonus.isClaimed = true;
        }

        public DailyBonus GetDailyBonus(int dayNum)
        {
            DailyBonus dailyBonus = dailyBonusList.Find(item => item.dayNum == dayNum);
            return dailyBonus;
        }

        public bool IsDailyBonusClaimed(int dayNum)
        {
            DailyBonus dailyBonus = GetDailyBonus(dayNum);
            return dailyBonus.isClaimed;
        }

        public int GetDailyBonusRewardCount(int dayNum)
        {
            DailyBonus dailyBonus = GetDailyBonus(dayNum);
            return dailyBonus.rewardCount;
        }
    }

    public class DailyBonus
    {
        public int dayNum;
        public int rewardCount;
        public bool isClaimed;
    }

    public enum ESignType
    {
        Signed,
        Sign,
        NotSigned
    }

}

