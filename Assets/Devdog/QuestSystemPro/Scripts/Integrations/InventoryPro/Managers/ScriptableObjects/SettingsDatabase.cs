#if INVENTORY_PRO

using Devdog.General;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public partial class SettingsDatabase
    {

        [Category("Inventory Pro Prefabs")]
        [Header("Inventory Pro Quest Prefabs")]
        public RewardRowUI inventoryProItemRewardRowUI;
        public RewardRowUI inventoryProStatRewardRowUI;
        public RewardRowUI inventoryProInventorySlotsRewardRowUI;

        [Header("Inventory Pro Task Prefabs")]
        public TaskProgressRowUI inventoryProGatherTaskRowUI;

    }
}

#endif