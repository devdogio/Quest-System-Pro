using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    public abstract class SimpleQuestEdgeConditionBase : SimpleEdgeCondition
    {
        [Required]
        public Asset<Quest>[] quests = new Asset<Quest>[0];

        public override ValidationInfo Validate(Dialogue dialogue)
        {
            if (quests.Any(o => o.val == null))
            {
                return new ValidationInfo(ValidationType.Error, "There's an empty quest object in the edge.");
            }

            return base.Validate(dialogue);
        }

        public override string FormattedString()
        {
            string questIDs = "";
            foreach (var quest in quests)
            {
                if (quest.val == null)
                {
                    continue;
                }

                questIDs += "#" + quest.val.ID + ", ";
            }

            return "Has active quests " + questIDs;
        }
    }
}
