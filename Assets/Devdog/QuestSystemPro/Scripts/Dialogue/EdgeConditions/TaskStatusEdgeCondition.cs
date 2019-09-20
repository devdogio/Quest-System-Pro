namespace Devdog.QuestSystemPro.Dialogue
{
    public class TaskStatusEdgeCondition : SimpleQuestEdgeConditionBase
    {
        public TaskStatus status = TaskStatus.Active;
        public string taskName = "";

        public override bool CanUse(Dialogue dialogue)
        {
            foreach (var quest in quests)
            {
                if (quest.val == null)
                {
                    continue;
                }

                var task = quest.val.GetTask(taskName);
                if (task != null && task.status != status)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
