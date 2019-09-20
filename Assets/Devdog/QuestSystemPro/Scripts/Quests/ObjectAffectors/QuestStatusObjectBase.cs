using System;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public abstract class QuestStatusObjectBase : MonoBehaviour
    {
        public QuestStatusDecorator questStatus;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            questStatus.RegisterCallbacks(OnQuestStatusChanged, OnTaskStatusChanged);
        }

        protected virtual void OnDestroy()
        {
            questStatus.UnRegisterCallbacks(OnQuestStatusChanged, OnTaskStatusChanged);
        }

        private void OnQuestStatusChanged(QuestStatus before, Quest self)
        {
            if (self.status == questStatus.questStatus)
            {
                OnStatusChangedCorrect(self);
            }
            else
            {
                OnStatusChangedInCorrect(self);
            }
        }

        private void OnTaskStatusChanged(TaskStatus before, TaskStatus after, Task self)
        {
            if (after == questStatus.taskStatus)
            {
                OnStatusChangedCorrect(self.owner);
            }
            else
            {
                OnStatusChangedInCorrect(self.owner);
            }
        }

        /// <summary>
        /// The status of the quest or task has changed to the specified filters. Do the 'positive' or 'correct' actions.
        /// </summary>
        /// <param name="self"></param>
        protected abstract void OnStatusChangedCorrect(Quest self);

        /// <summary>
        /// The status of the quest or task has changed and is no longer correct according to the filters. Do the 'negative' or 'incorrect' action.
        /// </summary>
        /// <param name="self"></param>
        protected virtual void OnStatusChangedInCorrect(Quest self)
        { }
    }
}
