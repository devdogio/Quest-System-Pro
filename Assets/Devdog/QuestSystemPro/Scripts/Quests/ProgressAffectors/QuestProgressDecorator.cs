using System;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public sealed class QuestProgressDecorator
    {
        public enum Type
        {
            Add,
            Set
        }

        [Required]
        [ForceCustomObjectPicker]
        public Quest quest;
        public string taskName;

        public Type type = Type.Add;
        public float progress;
        public bool useTaskProgressCap = true;

        public bool Execute()
        {
            if (useTaskProgressCap)
            {
                var task = quest.GetTask(taskName);
                Assert.IsNotNull(task, "Task with name '" + taskName + "' could not be found");
                if (task.progress >= task.progressCap)
                {
                    return false;
                }
            }

            switch (type)
            {
                case Type.Add:
                    return quest.ChangeTaskProgress(taskName, progress);
                case Type.Set:
                    return quest.SetTaskProgress(taskName, progress);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
