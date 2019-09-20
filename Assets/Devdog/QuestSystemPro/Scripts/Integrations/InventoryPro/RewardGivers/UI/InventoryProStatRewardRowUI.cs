#if INVENTORY_PRO

using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Integration.InventoryPro.UI
{
    public class InventoryProStatRewardRowUI : RewardRowUI
    {
        [Header("Options")]
        public bool overrideColor = false;
        public Color positiveColor = Color.green;
        public Color negativeColor = Color.red;

        [Header("UI Elements")]
        public Image icon;
        public Text statName;
        public Text statValue;

        public override void Repaint(IRewardGiver rewardGiver, Quest quest)
        {
            var r = (InventoryProStatRewardGiver) rewardGiver;
            var statDef = r.statDecorator.stat;

            if (icon != null)
            {
                icon.sprite = statDef.icon;
            }

            if (statName != null)
            {
                statName.text = statDef.name;
                statName.color = statDef.color;
            }

            if(statValue != null)
            {
                statValue.text = r.statDecorator.ToString();
                if (overrideColor)
                {
                    statValue.color = r.statDecorator.floatValue >= 0f ? positiveColor : negativeColor;
                }
                else
                {
                    statValue.color = statDef.color;
                }
            }
        }
    }
}

#endif