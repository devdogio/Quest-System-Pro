using System;
using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Set task progress on a quest.")]
    [Category("Custom/Flow control")]
    public class SetTaskProgressNode : ActionNodeBase
    {
        [ShowInNode]
        public TaskStatus status;

        [ShowInNode]
        [Required]
        public Asset<Quest> quest;

        [ShowInNode]
        [Required]
        public string taskKey = "Main";

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            var task = quest.val.GetTask(taskKey);

            switch (status)
            {
                case TaskStatus.InActive:
                    task.ResetProgress();
                    break;
                case TaskStatus.Active:
                    task.Activate();
                    break;
                case TaskStatus.Completed:
                    task.Complete(true);
                    break;
                case TaskStatus.Failed:
                    task.Fail();
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

            if (quest.val.GetTask(taskKey) == null)
            {
                return new ValidationInfo(ValidationType.Error, "Task " + taskKey + " does not exist on quest.");
            }

            return new ValidationInfo(ValidationType.Valid);
        }
    }
}