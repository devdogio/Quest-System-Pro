#if LOVE_HATE__

using System;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    [System.Serializable]
    public class LoveHateGatherTask : Task
    {
        public override TaskProgressRowUI taskUIPrefab
        {
            get { return QuestManager.instance.settingsDatabase.loveHateGatherTaskRowUI ?? QuestManager.instance.settingsDatabase.defaultTaskRowUI; }
        }

        public bool removeStatsOnComplete = true;

//        [Obsolete("Use other constructors instead.")]
        public LoveHateGatherTask()
        { }

        public LoveHateGatherTask(string key, float progressCap)
            : base(key, progressCap)
        { }

        public LoveHateGatherTask(string key, float progressCap, params IRewardGiver[] rewardGivers)
            : base(key, progressCap, rewardGivers)
        { }

        public override ConditionInfo CanComplete()
        {
            // Check if LoveHate has the right stats to complete this task
            if (removeStatsOnComplete && loveHateStat < progressCap)
            {
                return new ConditionInfo(false, QuestManager.instance.languageDatabase.canNotCompleteQuestInventoryIsFull);
            }

            return base.CanComplete();
        }

        public override bool GiveRewards(bool force = false)
        {
            bool success = base.GiveRewards(force);
            if (success && removeItemsOnComplete)
            {
                InventoryManager.RemoveItem(item.ID, (uint)progressCap, false);
            }

            return success;
        }

//        public override void NotifyQuestCompleted()
//        {
//            base.NotifyQuestCompleted();
//
//            if (removeItemsOnComplete)
//            {
//                InventoryManager.RemoveItem(item.val.ID, (uint)progressCap, false);
//            }
//        }

        public override string GetStatusMessage()
        {
            return string.Format(statusMessage.message, progress, progressNormalized, progressCap, item.name, item.description);
        }
    }
}

#endif