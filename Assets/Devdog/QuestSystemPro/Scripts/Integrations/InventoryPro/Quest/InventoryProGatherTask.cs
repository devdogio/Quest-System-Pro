#if INVENTORY_PRO

using System;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    [System.Serializable]
    public class InventoryProGatherTask : Task, IInventoryProTask
    {
        public override TaskProgressRowUI taskUIPrefab
        {
            get { return QuestManager.instance.settingsDatabase.inventoryProGatherTaskRowUI ?? QuestManager.instance.settingsDatabase.defaultTaskRowUI; }
        }

        [SerializeField]
        private InventoryItemBase _item;
        public InventoryItemBase item
        {
            get { return _item; }
        }


        public bool removeItemsOnComplete = true;

        public override Sprite icon
        {
            get { return item.icon; }
        }

        [NonSerialized]
        protected new Sprite _icon;


//        [Obsolete("Use other constructors instead.")]
        public InventoryProGatherTask()
        { }

        public InventoryProGatherTask(string key, float progressCap)
            : base(key, progressCap)
        { }

        public InventoryProGatherTask(string key, float progressCap, params IRewardGiver[] rewardGivers)
            : base(key, progressCap, rewardGivers)
        { }

        public override ConditionInfo CanComplete()
        {
            if (removeItemsOnComplete && InventoryManager.GetItemCount(item.ID, false) < progressCap)
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