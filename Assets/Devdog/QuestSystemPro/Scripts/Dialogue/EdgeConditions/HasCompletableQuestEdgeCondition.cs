using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class HasCompletableQuestEdgeCondition : SimpleQuestEdgeConditionBase
    {
        public override bool CanUse(Dialogue dialogue)
        {
            return quests.All(quest => quest != null && quest.val.CanComplete().status);
        }
    }
}
