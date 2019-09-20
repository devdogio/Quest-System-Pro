using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class QuestTimeHandler : IQuestTimeHandler
    {
        public bool failQuestWhenOutOfTime = true;

        public void OnTimerStarted(Task task)
        {

        }

        public void OnTimerUpdated(Task task)
        {
            
        }

        public void OnTimerStopped(Task task)
        {

        }

        public void OnReachedTimeLimit(Task task)
        {
            task.Fail();

            if (failQuestWhenOutOfTime)
            {
                DevdogLogger.LogVerbose("Quest cancelled because time limit was reached on required task (" + task.key + ").");
                task.owner.Cancel();
            }
        }
    }
}