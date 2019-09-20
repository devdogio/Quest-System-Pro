using Devdog.QuestSystemPro.UI;

namespace Devdog.QuestSystemPro
{
    public interface IRewardGiver
    {
        RewardRowUI rewardUIPrefab { get; }

        ConditionInfo CanGiveRewards(Quest quest);
        void GiveRewards(Quest quest);
    }
}