namespace Devdog.QuestSystemPro.UI
{
    public struct TaskPreviousProgressPair
    {
        public Task task;
        public float taskProgressBefore;

        public TaskPreviousProgressPair(float taskProgressBefore, Task task)
        {
            this.task = task;
            this.taskProgressBefore = taskProgressBefore;
        }
    }
}
