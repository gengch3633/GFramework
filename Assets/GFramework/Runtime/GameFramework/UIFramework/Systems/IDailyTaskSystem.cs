using Framework;
using GameFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public partial interface IDailyTaskSystem : ISystem
    {
        bool IsTaskCanRewarded(int taskId);

        bool IsTaskRewardClaimed(int taskId);

        void ClaimTaskReward(int taskId);

        void CompleteTask(int taskId);

        TaskInfo GetTaskInfo(int taskId);

        List<TaskInfo> GetAllDailyTasks();

        int GetRewardClaimedTaskCount();

        TaskInfo GetCanRewardedTask();
    }

    public partial class DailyTaskSystem : AbstractSystem, IDailyTaskSystem
    {
        private TaskGroup dailyTaskGroup;
        private List<TaskInfo> initDailyTasks;
        private IResourceSystem resourceSystem;
        private ILanguageSystem languageSystem;
        protected override void OnInit()
        {
            this.resourceSystem = this.GetSystem<IResourceSystem>();
            this.languageSystem = this.GetSystem<ILanguageSystem>();
            this.initDailyTasks = GameUtils.GetConfigInfos<TaskInfo>();
            this.dailyTaskGroup = ReadInfoWithReturnNull<TaskGroup>();
        }

        protected override void OnAfterInit()
        {
            base.OnAfterInit();

            if (dailyTaskGroup == null || (DateTime.Today - dailyTaskGroup.taskDate).Days != 0)
            {
                var randomTasks = initDailyTasks.ConvertAll(item=>item);
                var loopCount = randomTasks.Count;
                while(loopCount > 0)
                {
                    var randomIndex = UnityEngine.Random.Range(0, randomTasks.Count);
                    (randomTasks[0], randomTasks[randomIndex]) = (randomTasks[randomIndex], randomTasks[0]);
                    loopCount--;
                }

                var tasks = initDailyTasks.GetRange(0, 3);
                dailyTaskGroup = new TaskGroup(DateTime.Today, tasks);
                SaveTaskInfo();
            }
        }

        private void SaveTaskInfo()
        {
            SaveInfo(dailyTaskGroup);
        }

        public void CompleteTask(int taskId)
        {
            var allNowTaskIds = dailyTaskGroup.taskInfos.ConvertAll(item => item.dailyTaskId);
            if (!allNowTaskIds.Contains(taskId))
                return;

            TaskInfo dailyTask = GetTaskInfo(taskId);
            dailyTask.taskCompleteCount += 1;

            dailyTask.taskCompleteCount = Mathf.Min(dailyTask.taskCompleteCount, dailyTask.taskExpectCount);
            SaveTaskInfo();
        }

        public bool IsTaskCanRewarded(int taskId)
        {
            TaskInfo dailyTask = GetTaskInfo(taskId);
            return dailyTask.taskCompleteCount >= dailyTask.taskExpectCount;
        }

        public TaskInfo GetTaskInfo(int taskId)
        {
            TaskInfo task = dailyTaskGroup.taskInfos.Find(item => item.dailyTaskId == taskId);
            return task;
        }

        public bool IsTaskRewardClaimed(int taskId)
        {
            TaskInfo dailyTask = GetTaskInfo(taskId);
            return dailyTask.isRewardClaimed;
        }

        public void ClaimTaskReward(int taskId)
        {
            TaskInfo dailyTask = GetTaskInfo(taskId);
            dailyTask.isRewardClaimed = true;
            SaveTaskInfo();
        }

        public List<TaskInfo> GetAllDailyTasks()
        {
            return dailyTaskGroup.taskInfos;
        }

        public TaskInfo GetCanRewardedTask()
        {
            TaskInfo dailyTask1 = dailyTaskGroup.taskInfos.Find(item => IsTaskCanRewarded(item.dailyTaskId) && !item.isRewardClaimed);
            TaskInfo dailyTask =  dailyTask1;
            return dailyTask;
        }

        public int GetRewardClaimedTaskCount()
        {
            var dailyTasks = GetAllDailyTasks();
            var allClaimedTaskCount = dailyTasks.FindAll(item => item.isRewardClaimed).Count;
            return allClaimedTaskCount;
        }
    }

    public class TaskGroup
    {
        public DateTime taskDate;
        public List<TaskInfo> taskInfos;

        public TaskGroup()
        {
        }

        public TaskGroup(DateTime taskDate, List<TaskInfo> dailyTasks)
        {
            this.taskDate = taskDate;
            this.taskInfos = dailyTasks;
        }
    }

    public class TaskInfo
    {
        public int dailyTaskId;
        public string taskDesc;
        public int taskCompleteCount;
        public int taskExpectCount;

        public int rewardCount;
        public bool isRewardClaimed;

        public TaskInfo()
        {
        }

        public TaskInfo(int dailyTaskId, string taskDesc, int taskCompleteCount, int taskExpectCount, int rewardCount, bool isRewardClaimed)
        {
            this.dailyTaskId = dailyTaskId;
            this.taskDesc = taskDesc;
            this.taskCompleteCount = taskCompleteCount;
            this.taskExpectCount = taskExpectCount;
            this.rewardCount = rewardCount;
            this.isRewardClaimed = isRewardClaimed;
        }
    }
}

