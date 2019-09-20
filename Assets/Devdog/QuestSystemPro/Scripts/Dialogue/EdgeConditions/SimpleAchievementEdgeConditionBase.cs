using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    public abstract class SimpleAchievementEdgeConditionBase : SimpleEdgeCondition
    {
        [Required]
        public Asset<Achievement>[] achievements = new Asset<Achievement>[0];

        public override ValidationInfo Validate(Dialogue dialogue)
        {
            if (achievements.Any(o => o.val == null))
            {
                return new ValidationInfo(ValidationType.Error, "There's an empty achievement object in the edge.");
            }

            return base.Validate(dialogue);
        }

        public override string FormattedString()
        {
            string achievementIDs = "";
            foreach (var achievement in achievements)
            {
                if (achievement.val == null)
                {
                    continue;
                }

                achievementIDs += "#" + achievement.val.ID + ", ";
            }

            return "Has active achievements " + achievementIDs;
        }
    }
}
