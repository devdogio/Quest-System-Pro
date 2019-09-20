#if INVENTORY_PRO

using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Integration.InventoryPro.UI
{
    public class InventoryProInventorySlotsRewardRowUI : RewardRowUI
    {
        public string format = "{0} extra slots";

        [Header("UI Elements")]
        public Text slotCount;
        

        public override void Repaint(IRewardGiver rewardGiver, Quest quest)
        {
            var r = (InventoryProInventorySlotsRewardGiver) rewardGiver;

            if (slotCount != null)
            {
                slotCount.text = string.Format(format, r.extraSlots);
            }
        }
    }
}

#endif