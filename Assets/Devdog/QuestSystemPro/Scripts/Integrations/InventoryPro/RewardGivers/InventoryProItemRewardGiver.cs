#if INVENTORY_PRO

using Devdog.InventoryPro;
using Devdog.QuestSystemPro.UI;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public partial class InventoryProItemRewardGiver : IRewardGiver
    {
        public ItemAmountRow reward;

        public virtual RewardRowUI rewardUIPrefab
        {
            get { return QuestManager.instance.settingsDatabase.inventoryProItemRewardRowUI; }
        }

        public ConditionInfo CanGiveRewards(Quest quest)
        {
            var canAdd = InventoryManager.CanAddItem(reward);
            if (canAdd == false)
            {
                return new ConditionInfo(false, QuestManager.instance.languageDatabase.canNotCompleteQuestInventoryIsFull);
            }

            return ConditionInfo.success;
        }

        public void GiveRewards(Quest quest)
        {
            var inst = UnityEngine.Object.Instantiate<InventoryItemBase>(reward.item);
            inst.currentStackSize = reward.amount;

            InventoryManager.AddItem(inst);
        }
    }
}

#endif