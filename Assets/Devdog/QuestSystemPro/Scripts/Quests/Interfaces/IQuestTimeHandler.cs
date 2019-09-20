namespace Devdog.QuestSystemPro
{
    public interface IQuestTimeHandler
    {

        /// <summary>
        /// The timer has started
        /// </summary>
        /// <param name="task"></param>
        void OnTimerStarted(Task task);

        /// <summary>
        /// The timer has updated. The 'delta' time is variable; Always calculate your own deltaTime don't re-use Time.deltaTime;
        /// </summary>
        /// <param name="task"></param>
        void OnTimerUpdated(Task task);

        /// <summary>
        /// The timer has stopped. This does not mean the player has failed the task.
        /// </summary>
        /// <param name="task"></param>
        void OnTimerStopped(Task task);

        /// <summary>
        /// The time limit for the task has been reached.
        /// This is ONLY called IF the task runs out of time. If the task is completed before running out of time the timer is stopped and this method will neve be called.
        /// </summary>
        /// <param name="task"></param>
        void OnReachedTimeLimit(Task task);
    }
}