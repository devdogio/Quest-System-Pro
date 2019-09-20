using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Category("Devdog/Quests")]
    public class ChangeQuestStatusNode : ActionNodeBase
    {
        [ShowInNode]
        [Required]
        public Asset<Quest>[] quests;

        [ShowInNode]
        public QuestStatusAction status;


        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            foreach (var quest in quests)
            {
                quest.val.DoAction(status);
            }

            Finish(true);
        }

        public override ValidationInfo Validate()
        {
            if (quests.Any(o => o == null || o.val == null))
            {
                return new ValidationInfo(ValidationType.Error, "One of the quest fields is empty!");
            }

            return base.Validate();
        }
    }
}