#if INVENTORY_PRO

using Devdog.General;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public partial class SettingsDatabase
    {

        [Category("Love/Hate Prefabs")]
        [Header("Love/Hate Quest Prefabs")]
        public RewardRowUI loveHateRewardRowUI;

        [Header("Love/Hate Task Prefabs")]
        public TaskProgressRowUI loveHateGatherTaskRowUI;

    }
}

#endif