#if INVENTORY_PRO

using Devdog.InventoryPro;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.InventoryPro.UI
{
    public class InventoryProItemRewardRowUI : RewardRowUI
    {
        [Header("UI Elements")]
#if !INVENTORY_PRO_LEGACY
        public ItemCollectionSlotUIStatic wrapper;
#else
        public InventoryUIItemWrapperStatic wrapper;
#endif

        public override void Repaint(IRewardGiver rewardGiver, Quest quest)
        {
            var r = (InventoryProItemRewardGiver) rewardGiver;

            r.reward.item.currentStackSize = r.reward.amount;
            wrapper.item = r.reward.item;
            wrapper.Repaint();
            r.reward.item.currentStackSize = 1; // Restore
        }
    }
}

#endif