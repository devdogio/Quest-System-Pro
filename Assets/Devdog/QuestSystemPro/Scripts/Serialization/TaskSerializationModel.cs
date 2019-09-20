using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class TaskSerializationModel
    {
        public string key;
        public float progress;
        public DateTime? startTime;
        public TaskStatus status;
        public bool gaveRewards;

        private static readonly FieldInfo _progressField;
        private static readonly FieldInfo _startTimeField;
        private static readonly FieldInfo _statusField;
        private static readonly FieldInfo _gaveRewardsField;

        static TaskSerializationModel()
        {
            var t = typeof(Task);
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            _progressField = t.GetField("_progress", bindingFlags);
            _startTimeField = t.GetField("_startTime", bindingFlags);
            _statusField = t.GetField("_status", bindingFlags);
            _gaveRewardsField = t.GetField("_gaveRewards", bindingFlags);

            Assert.IsNotNull(_progressField);
            Assert.IsNotNull(_startTimeField);
            Assert.IsNotNull(_statusField);
            Assert.IsNotNull(_gaveRewardsField);
        }

        public TaskSerializationModel()
        {
            
        }

        public TaskSerializationModel(Task task)
        {
            LoadFrom(task);
        }

        public void LoadFrom(Task task)
        {
            key = task.key;
            progress = task.progress;
            startTime = task.startTime;
            status = task.status;
            gaveRewards = task.gaveRewards;
        }

        public void LoadTo(Task task)
        {
            _progressField.SetValue(task, progress);
            _startTimeField.SetValue(task, startTime);
            _statusField.SetValue(task, status);
            _gaveRewardsField.SetValue(task, gaveRewards);
        }
    }
}
