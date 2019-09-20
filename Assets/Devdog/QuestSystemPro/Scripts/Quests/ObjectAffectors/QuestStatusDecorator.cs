using System;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public sealed class QuestStatusDecorator
    {
        [Required]
        [ForceCustomObjectPicker]
        public Quest quest;
        public QuestStatus questStatus;

        public bool useTaskStatus;
        public string taskName = "Main";
        public TaskStatus taskStatus = TaskStatus.Active;

        /// <summary>
        /// When true the register callbacks will fire an initial callback to synchornize the object with it's current state.
        /// <remarks>When registering the event deserialization already happend, so events won't fire until the status has changed. 
        /// To fix this issue the RegisterCallbacks triggers the callback directly to synchornize the actions. This can be unwanted behavior in some situations.</remarks>
        /// </summary>
        [NonSerialized]
        public bool syncStateOnCallbackRegistration = true;

        private Task _task;
        public void RegisterCallbacks(Quest.StatusChanged questStatusChanged, Task.StatusChanged taskStatusChanged)
        {
            if (useTaskStatus)
            {
                _task = quest.GetTask(taskName);
                Assert.IsNotNull(_task, "Couldn't find task with name '" + taskName + "' on " + GetType().Name);

                _task.OnStatusChanged += taskStatusChanged;
                if (syncStateOnCallbackRegistration)
                {
                    taskStatusChanged(TaskStatus.InActive, _task.status, _task);
                }
            }
            else
            {
                quest.OnStatusChanged += questStatusChanged;
                if (syncStateOnCallbackRegistration)
                {
                    questStatusChanged(QuestStatus.InActive, quest);
                }
            }
        }

        public void UnRegisterCallbacks(Quest.StatusChanged questStatusChanged, Task.StatusChanged taskStatusChanged)
        {
            if (useTaskStatus)
            {
                Assert.IsNotNull(_task);
                _task.OnStatusChanged -= taskStatusChanged;
            }
            else
            {
                quest.OnStatusChanged -= questStatusChanged;
            }
        }

        public bool IsCurrentStatusCorrect()
        {
            if (useTaskStatus)
            {
                var task = quest.GetTask(taskName);
                Assert.IsNotNull(_task, "Couldn't find task with name '" + taskName + "' on " + GetType().Name);

                return task.status == taskStatus;
            }

            return quest.status == questStatus;
        }
    }
}
