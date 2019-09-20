using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    public class DefaultRewardRowUI : RewardRowUI
    {
        [Header("UI References")]
        public Text key;
        public Text val;

        public override void Repaint(IRewardGiver rewardGiver, Quest quest)
        {
            Assert.IsTrue(rewardGiver is INamedRewardGiver, "To use the default rewardRowUI the rewardGiver MUST be of type INamedRewardGiver - " + rewardGiver.GetType().Name + " given.");

            var named = (INamedRewardGiver) rewardGiver;

            key.text = named.name;
            val.text = named.ToString();
        }
    }
}