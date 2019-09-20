#if LOVE_HATE

using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using PixelCrushers.LoveHate;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    public class AffinityEdgeCondition : SimpleEdgeCondition
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

        public override bool CanUse(Dialogue.Dialogue dialogue)
        {
            var currentAffinity = QuestSystemLoveHateBridgeManager.factionManager.GetAffinity(judgeFactionName, subjectFactionName);
            switch (filterType)
            {
                case FilterType.HigherThanOrEqual:
                    return currentAffinity >= affinity;
                case FilterType.LowerThanOrEqual:
                    return currentAffinity <= affinity;
                default:
                {
                    DevdogLogger.LogWarning("Filter type of " + filterType + " not found, please report this error + Stack trace.");
                    return false;
                }
            }
        }

        public override string FormattedString()
        {
            string symbol = "";
            var currentAffinity = QuestSystemLoveHateBridgeManager.factionManager.GetAffinity(judgeFactionName, subjectFactionName);
            switch (filterType)
            {
                case FilterType.HigherThanOrEqual:
                    symbol = ">=";
                    break;
                case FilterType.LowerThanOrEqual:
                    symbol = "<=";
                    break;
                default:
                    symbol = "??";
                    break;
            }

            return string.Format("Affinity {0} {1} {2}", currentAffinity, symbol, affinity);
        }
    }
}

#endif