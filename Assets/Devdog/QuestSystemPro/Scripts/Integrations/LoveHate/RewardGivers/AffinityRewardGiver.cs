#if LOVE_HATE

using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.UI;
using PixelCrushers.LoveHate;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    public partial class AffinityRewardGiver : IRewardGiver
    {
        protected enum ChangeValueType
        {
            Change,
            Set
        }

        [Required]
        [ShowInNode]
        public string judgeFactionName = "Some NPC";

        [Required]
        [ShowInNode]
        public string subjectFactionName = "Player";

        [ShowInNode]
        [Range(-100f, 100f)]
        public float affinity;

        [ShowInNode]
        [SerializeField]
        protected ChangeValueType type = ChangeValueType.Change;


        public virtual RewardRowUI rewardUIPrefab {
            get { return QuestManager.instance.settingsDatabase.loveHateRewardRowUI; }
        }

        public ConditionInfo CanGiveRewards(Quest quest)
        {
            return ConditionInfo.success;
        }

        public void GiveRewards(Quest quest)
        {
            switch (type)
            {
                case ChangeValueType.Change:
                    QuestSystemLoveHateBridgeManager.factionManager.ModifyPersonalAffinity(judgeFactionName, subjectFactionName, affinity);
                    break;
                case ChangeValueType.Set:
                    QuestSystemLoveHateBridgeManager.factionManager.SetPersonalAffinity(judgeFactionName, subjectFactionName, affinity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

#endif