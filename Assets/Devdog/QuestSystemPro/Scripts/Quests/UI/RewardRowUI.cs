using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    // Can't be abstract, otherwise Unity won't serialize it anymore.
    public class RewardRowUI : MonoBehaviour
    {
        public virtual void Repaint(IRewardGiver rewardGiver, Quest quest) { }
    }
}