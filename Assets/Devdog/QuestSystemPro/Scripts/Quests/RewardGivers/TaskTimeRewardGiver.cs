using System;
using Devdog.General;
using Devdog.QuestSystemPro.UI;

namespace Devdog.QuestSystemPro
{
    public class TaskTimeRewardGiver : INamedRewardGiver
    {
        public string taskName;
        public float addTimeInSeconds;

        public string name
        {
            get { return "Task '" + taskName + "' time"; }
        }

        public RewardRowUI rewardUIPrefab
        {
            get { return QuestManager.instance.settingsDatabase.defaultRewardRowUI; }
        }

        public ConditionInfo CanGiveRewards(Quest quest)
        {
            return ConditionInfo.success;
        }

        public void GiveRewards(Quest quest)
        {
            var task = quest.GetTask(taskName);
            if (task == null)
            {
                DevdogLogger.LogWarning("Task " + taskName + " not found on quest " + quest);
                return;
            }

            task.timeLimitInSeconds += addTimeInSeconds;
            DevdogLogger.LogVerbose("Gave task " + taskName + " " + addTimeInSeconds + " extra seconds (rewardGiver)");
        }

        public override string ToString()
        {
            return TimeSpan.FromSeconds(addTimeInSeconds).ToString();
        }
    }
}
