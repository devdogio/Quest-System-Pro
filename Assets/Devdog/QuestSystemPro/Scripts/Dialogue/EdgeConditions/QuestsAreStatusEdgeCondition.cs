using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class QuestsAreStatusEdgeCondition : SimpleQuestEdgeConditionBase
    {
        public QuestStatus status = QuestStatus.InActive;

        public override bool CanUse(Dialogue dialogue)
        {
            return quests.All(quest => quest.val != null && quest.val.status == status);
        }
    }
}
