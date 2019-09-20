#if INVENTORY_PRO

using System.Collections.Generic;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public class QuestSystemInventoryProBridgeManager : MonoBehaviour
    {
        private Dictionary<IInventoryProTask, Task> _inventoryProQuestTasks = new Dictionary<IInventoryProTask, Task>();
        private Dictionary<IInventoryProTask, Task> _inventoryProAchievementTasks = new Dictionary<IInventoryProTask, Task>();

        protected virtual void Awake()
        {
            GetInventoryProTasks(QuestManager.instance.quests, _inventoryProQuestTasks);
            GetInventoryProTasks(QuestManager.instance.achievements, _inventoryProAchievementTasks);

            foreach (var task in _inventoryProQuestTasks)
            {
                task.Value.OnStatusChanged += TaskOnStatusChanged;
            }
        }

        protected virtual void Start()
        {
            PlayerManager.instance.OnPlayerChanged += InstanceOnPlayerChanged;
            if (PlayerManager.instance.currentPlayer != null)
            {
                InstanceOnPlayerChanged(null, PlayerManager.instance.currentPlayer);
            }
        }

        protected virtual void OnDestroy()
        {
            if(PlayerManager.instance != null)
            {
                PlayerManager.instance.OnPlayerChanged -= InstanceOnPlayerChanged;
            }
        }

        protected virtual void GetInventoryProTasks<T>(IEnumerable<T> l, Dictionary<IInventoryProTask, Task> appendTo) where T: Quest
        {
            foreach (var obj in l)
            {
                foreach (var task in obj.tasks)
                {
                    var iTask = task as IInventoryProTask;
                    if (iTask != null)
                    {
                        appendTo.Add(iTask, task);
                    }
                }
            }
        }

        protected virtual void TaskOnStatusChanged(TaskStatus before, TaskStatus after, Task self)
        {
            if (after == TaskStatus.Active)
            {
                // Check inventory for existing items
                var iTask = (IInventoryProTask)self;
                self.SetProgress(InventoryManager.GetItemCount(iTask.item.ID, false));
            }
        }

        private void InstanceOnPlayerChanged(Player oldPlayer, Player newPlayer)
        {
            if (oldPlayer != null)
            {
                foreach (var inv in oldPlayer.inventoryPlayer.inventoryCollections)
                {
                    inv.OnAddedItem -= InvOnAddedItem;
                    inv.OnRemovedItem -= InvOnRemovedItem;
                }
            }

            if (newPlayer != null)
            {
                foreach (var inv in newPlayer.inventoryPlayer.inventoryCollections)
                {
                    inv.OnAddedItem += InvOnAddedItem;
                    inv.OnRemovedItem += InvOnRemovedItem;
                }
            }
        }
        
        private void InvOnAddedItem(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
        {
            var itemID = items.First().ID;
            UpdateQuestTaskProgress(itemID, amount);
            UpdateAchievementTaskProgress(itemID, amount);
        }

        private void InvOnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            UpdateQuestTaskProgress(itemID, amount);
        }

        private void UpdateQuestTaskProgress(uint itemID, uint amountAdded)
        {
            foreach (var t in _inventoryProQuestTasks)
            {
                if (t.Value.status != TaskStatus.Active)
                {
                    continue;
                }

                if (t.Key.item.ID == itemID)
                {
                    t.Value.SetProgress(InventoryManager.GetItemCount(itemID, false));
                }
            }
        }

        private void UpdateAchievementTaskProgress(uint itemID, uint amountAdded)
        {
            foreach (var t in _inventoryProAchievementTasks)
            {
                if (t.Key.item.ID == itemID)
                {
                    t.Value.ChangeProgress(amountAdded);
                }
            }
        }
    }
}

#endif