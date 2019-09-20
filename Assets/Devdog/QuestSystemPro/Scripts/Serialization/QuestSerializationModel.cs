using System.Reflection;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class QuestSerializationModel
    {
        public int ID;
        public int repeatedTimes;
        public QuestStatus status;

        public TaskSerializationModel[] tasks = new TaskSerializationModel[0];


        private static readonly FieldInfo _repeatedTimesField;
        private static readonly FieldInfo _statusField;
        static QuestSerializationModel()
        {
            var t = typeof (Quest);
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            _repeatedTimesField = t.GetField("_repeatedTimes", bindingFlags);
            _statusField = t.GetField("_status", bindingFlags);

            Assert.IsNotNull(_repeatedTimesField);
            Assert.IsNotNull(_statusField);
        }


        public QuestSerializationModel()
        {
            
        }

        public QuestSerializationModel(Quest quest)
        {
            LoadFrom(quest);
        }

        /// <summary>
        /// Load data from the given quest into this serialization model.
        /// </summary>
        /// <param name="quest"></param>
        public void LoadFrom(Quest quest)
        {
            ID = quest.ID;
            repeatedTimes = quest.repeatedTimes;
            status = quest.status;

            tasks = new TaskSerializationModel[quest.tasks.Length];
            for (int i = 0; i < quest.tasks.Length; i++)
            {
                tasks[i] = new TaskSerializationModel(quest.tasks[i]);
            }
        }

        /// <summary>
        /// Loads the data from this datamodel to the given quest.
        /// </summary>
        public void LoadTo(Quest quest)
        {
            _repeatedTimesField.SetValue(quest, repeatedTimes);
            _statusField.SetValue(quest, status);

            for (int i = 0; i < tasks.Length; i++)
            {
                if (quest.tasks.Length <= i)
                {
                    // Added tasks? Out of range...
                    continue;
                }

                tasks[i].LoadTo(quest.tasks[i]);
            }
        }
    }
}
