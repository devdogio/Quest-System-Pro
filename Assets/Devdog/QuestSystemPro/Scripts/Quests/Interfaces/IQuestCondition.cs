namespace Devdog.QuestSystemPro
{
    public interface IQuestCondition
    {
        ConditionInfo CanActivateQuest(Quest quest);
        ConditionInfo CanCancelQuest(Quest quest);
        ConditionInfo CanDeclineQuest(Quest quest);
        ConditionInfo CanDiscoverQuest(Quest quest);
        ConditionInfo CanCompleteQuest(Quest quest);
    }
}