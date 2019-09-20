#if INVENTORY_PRO

using Devdog.General;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.UI;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public partial class InventoryProStatRewardGiver : IRewardGiver
    {
        [HideGroup]
        [HideTypePicker]
        public StatDecorator statDecorator;

        public virtual RewardRowUI rewardUIPrefab {
            get { return QuestManager.instance.settingsDatabase.inventoryProStatRewardRowUI; }
        }

        public InventoryPlayer currentPlayer
        {
            get { return PlayerManager.instance.currentPlayer.inventoryPlayer; }
        }

        public ConditionInfo CanGiveRewards(Quest quest)
        {
            return ConditionInfo.success;
        }

        public void GiveRewards(Quest quest)
        {
            var p = statDecorator.stat;
            var stat = currentPlayer.stats.Get(p.category, p.name);
            if (stat == null)
            {
                DevdogLogger.LogWarning("Stat with ID " + p.ID + " not found.");
                return;
            }

            currentPlayer.stats.Set(statDecorator);
        }
    }
}

#endif