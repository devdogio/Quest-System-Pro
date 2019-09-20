using System;
using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Set quest progress on a quest.")]
    [Category("Devdog/Quests")]
    public class SetQuestProgressNode : ActionNodeBase
    {
        public enum Type
        {
            Add,
            Set
        }

        [ShowInNode]
        public Type type;

        [ShowInNode]
        [Required]
        public Asset<Quest> quest;

        [ShowInNode]
        [Required]
        public string questTask = "Main";

        [ShowInNode]
        public float amount;


        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            switch (type)
            {
                case Type.Add:
                    quest.val.ChangeTaskProgress(questTask, amount);
                    break;
                case Type.Set:
                    quest.val.SetTaskProgress(questTask, amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Finish(true);
        }

        public override ValidationInfo Validate()
        {
            if (quest.val == null)
            {
                return new ValidationInfo(ValidationType.Error, "Quest field is empty");
            }

            if (quest.val.GetTask(questTask) == null)
            {
                return new ValidationInfo(ValidationType.Error, "Task " + questTask + " does not exist on quest.");
            }

            return new ValidationInfo(ValidationType.Valid);
        }
    }
}