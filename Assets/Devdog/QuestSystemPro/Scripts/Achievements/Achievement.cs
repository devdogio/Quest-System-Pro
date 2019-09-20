using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class Achievement : Quest
    {
        public new delegate void StatusChanged(QuestStatus before, Achievement achievement);
        public new delegate void TaskReachedTimeLimit(Task task, Achievement achievement);
        public new delegate void TaskStatusChanged(TaskStatus before, Task task, Achievement achievement);
        public new delegate void TaskProgressChanged(float taskProgressBefore, Task task, Achievement achievement);

        public new event Achievement.StatusChanged OnStatusChanged;
        public new event Achievement.TaskReachedTimeLimit OnTaskReachedTimeLimit;
        public new event Achievement.TaskProgressChanged OnTaskProgressChanged;
        public new event Achievement.TaskStatusChanged OnTaskStatusChanged;


        public new static Achievement Create(int id = 0)
        {
            return Create(QuestManager.instance.localIdentifier, id);
        }

        public new static Achievement Create(ILocalIdentifier localIdentifier, int id = 0)
        {
            var achievement = CreateInstance<Achievement>();
            achievement.ID = id;
            achievement.localIdentifier = localIdentifier;

            return achievement;
        }





        #region Notifies

        protected override void DoNotifyReachedTimeLimit(Task task)
        {
            QuestManager.instance.NotifyAchievementTaskReachedTimeLimit(task, this);
            if (OnTaskReachedTimeLimit != null)
            {
                OnTaskReachedTimeLimit(task, this);
            }
        }

        protected override void DoNotifyTaskProgressChanged(float before, Task task)
        {
            QuestManager.instance.NotifyAchievementTaskProgressChanged(before, task, this);
            if (OnTaskProgressChanged != null)
            {
                OnTaskProgressChanged(before, task, this);
            }
        }

        protected override void DoNotifyTaskStatusChanged(TaskStatus before, TaskStatus after, Task task)
        {
            QuestManager.instance.NotifyAchievementTaskStatusChanged(before, after, task, this);
            if (OnTaskStatusChanged != null)
            {
                OnTaskStatusChanged(before, task, this);
            }
        }

        protected override void DoNotifyQuestStatusChanged(QuestStatus before)
        {
            QuestManager.instance.NotifyAchievementStatusChanged(before, this);
            if (OnStatusChanged != null)
            {
                OnStatusChanged(before, this);
            }
        }

        #endregion





        /// <summary>
        /// Achievements become active auto. when you set it's progress.
        /// </summary>
        public override bool CanSetTaskProgress(string key, float value)
        {
            return true;
        }

        public override bool SetTaskProgress(string key, float value)
        {
            if (status == QuestStatus.InActive || status == QuestStatus.Cancelled)
            {
                DevdogLogger.LogVerbose("Setting achievement task progress, but achievement was not activated yet. Auto. activating achievement...");
                Activate();
            }

            return base.SetTaskProgress(key, value);
        }

        public override bool Cancel()
        {
            DevdogLogger.LogWarning("Can't cancel achievements. If you'd like to rest progress call achievement.ResetProgress() instead.");
            return false;
        }

        public override bool Decline()
        {
            DevdogLogger.LogWarning("Can't decline achievements. If you'd like to rest progress call achievement.ResetProgress() instead.");
            return false;
        }
    }
}
