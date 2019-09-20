#if LOVE_HATE

using Devdog.General.Localization;
using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Integration.LoveHate.UI
{
    public class LoveHateRewardGiverUI : RewardRowUI
    {
        [Header("Options")]
        public Color positiveColor = Color.green;
        public Color negativeColor = Color.red;

        [Header("UI Elements")]
        public Text statName;
        public Text statValue;

        public LocalizedString affinity = new LocalizedString("Devdog_LoveHate_Affinity");

        public override void Repaint(IRewardGiver rewardGiver, Quest quest)
        {
            var r = (AffinityRewardGiver) rewardGiver;

            if (statName != null)
            {
                statName.text = string.Format("{0} {1}", r.judgeFactionName, affinity.message);
            }

            if(statValue != null)
            {
                statValue.text = (r.affinity > 0 ? "+" : "-") + r.affinity;
                if (r.affinity > 0f)
                {
                    statValue.color = positiveColor;
                }
                else if(r.affinity < 0f)
                {
                    statValue.color = negativeColor;
                }
            }
        }
    }
}

#endif