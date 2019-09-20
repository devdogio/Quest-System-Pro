using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class HasActiveQuestEdgeCondition : SimpleQuestEdgeConditionBase
    {
        public bool onlyUncompletable = false;

        public override bool CanUse(Dialogue dialogue)
        {
            if (onlyUncompletable)
            {
                return quests.All(quest => quest != null && QuestManager.instance.HasActiveQuest(quest.val) && quest.val.CanComplete() == false);
            }

            return quests.All(quest => quest != null && QuestManager.instance.HasActiveQuest(quest.val));
        }
    }
}
