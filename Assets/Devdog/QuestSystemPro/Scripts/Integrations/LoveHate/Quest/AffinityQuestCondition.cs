#if LOVE_HATE

using System;
using Devdog.General;
using Devdog.General.Localization;
using PixelCrushers.LoveHate;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    [System.Serializable]
    public class AffinityQuestCondition : IQuestCondition
    {
        protected enum FilterType
        {
            HigherThanOrEqual,
            LowerThanOrEqual,
        }

        public string judgeFactionName = "Some NPC";
        public string subjectFactionName = "Player";

        [Range(-100f, 100f)]
        public float affinity = 10;

        [SerializeField]
        protected FilterType filterType;

        /// <summary>
        /// What quest status action is being checked? This will most likely be Activate.
        /// When the quest is activated afinity is tested.
        /// </summary>
        public QuestStatusAction status = QuestStatusAction.Activate;


        public LocalizedString affinityToLowString = new LocalizedString("Devdog_LoveHate_Quest_AffinityToLow") { message = "Affinity of {0} is to low, requires {1} or higher." };
        public LocalizedString affinityToHighString = new LocalizedString("Devdog_LoveHate_Quest_AffinityToHigh") { message = "Affinity of {0} is to high, requires {1} or less." };


        private ConditionInfo HasAffinity(QuestStatusAction currentAction)
        {
            if (currentAction != status)
            {
                // Not the status we're testing, this condition doesn't apply, always accept.
                return ConditionInfo.success;
            }

            var currentAffinity = QuestSystemLoveHateBridgeManager.factionManager.GetAffinity(judgeFactionName, subjectFactionName);
            switch (filterType)
            {
                case FilterType.HigherThanOrEqual:
                    {
                        if (currentAffinity >= affinity)
                        {
                            return ConditionInfo.success;
                        }

                        return new ConditionInfo(false, new MultiLangString("", string.Format(affinityToLowString.message, currentAffinity, affinity)));
                    }
                case FilterType.LowerThanOrEqual:
                    {
                        if (currentAffinity <= affinity)
                        {
                            return ConditionInfo.success;
                        }

                        return new ConditionInfo(false, new MultiLangString("", string.Format(affinityToHighString.message, currentAffinity, affinity)));
                    }
                default:
                    {
                        DevdogLogger.LogWarning("Filter type of " + filterType + " not found, please report this error + Stack trace.");
                        return new ConditionInfo(false);
                    }
            }
        }


        public ConditionInfo CanActivateQuest(Quest quest)
        {
            return HasAffinity(QuestStatusAction.Activate);
        }

        public ConditionInfo CanCancelQuest(Quest quest)
        {
            return HasAffinity(QuestStatusAction.Cancel);
        }

        public ConditionInfo CanDeclineQuest(Quest quest)
        {
            return ConditionInfo.success;
        }

        public ConditionInfo CanDiscoverQuest(Quest quest)
        {
            return ConditionInfo.success;
        }

        public ConditionInfo CanCompleteQuest(Quest quest)
        {
            return HasAffinity(QuestStatusAction.Complete);
        }
    }
}

#endif