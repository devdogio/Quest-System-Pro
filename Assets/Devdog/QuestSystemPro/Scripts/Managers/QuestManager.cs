using System;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Managers/Quest Manager")]
    public partial class QuestManager : MonoBehaviour
    {
        public event Quest.StatusChanged OnQuestStatusChanged;
        public event Quest.TaskReachedTimeLimit OnQuestTaskReachedTimeLimit;
        public event Quest.TaskProgressChanged OnQuestTaskProgressChanged;
        public event Quest.TaskStatusChanged OnQuestTaskStatusChanged;

        public event Achievement.StatusChanged OnAchievementStatusChanged;
        public event Achievement.TaskReachedTimeLimit OnAchievementTaskReachedTimeLimit;
        public event Achievement.TaskProgressChanged OnAchievementTaskProgressChanged;
        public event Achievement.TaskStatusChanged OnAchievementTaskStatusChanged;


        private static QuestManager _instance;
        public static QuestManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<QuestManager>();
                }

                return _instance;
            }
        }

        [NonSerialized]
        protected Dictionary<ILocalIdentifier, QuestsContainer> questStates = new Dictionary<ILocalIdentifier, QuestsContainer>();

        public Quest[] quests {
            get
            {
                if(questDatabase == null)
                    return new Quest[0];

                return questDatabase.quests;
            }
            set
            {
                if (questDatabase != null)
                    questDatabase.quests = value;
            }
        }

        public Achievement[] achievements
        {
            get
            {
                if (questDatabase == null)
                    return new Achievement[0];

                return questDatabase.achievements;
            }
            set
            {
                if (questDatabase != null)
                    questDatabase.achievements = value;
            }
        }


        private ILocalIdentifier _localIdentifier = new LocalIdentifier("0");
        /// <summary>
        /// The local identifier that belongs to this client.
        /// </summary>
        public ILocalIdentifier localIdentifier { 
            get
            {
                return _localIdentifier;
            }
            set
            {
                _localIdentifier = value;
            }
        }

        [Header("Databases")]
        [Required]
        public LanguageDatabase languageDatabase;

        [Required]
        public SettingsDatabase settingsDatabase;

        [Required]
        public QuestDatabase questDatabase;

        [Header("Scene references")]
        [Required]
        public QuestWindowUI questWindowUI;

        public IQuestGiver currentQuestGiver { get; set; }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Start()
        { }

        protected virtual void Init()
        {
            Assert.IsNotNull(languageDatabase, "Language database is not set on QuestManager! This is required.");
            Assert.IsNotNull(settingsDatabase, "Settings database is not set on QuestManager! This is required.");
            Assert.IsNotNull(questDatabase, "Quest database is not set on QuestManager! This is required.");

            foreach (var t in quests)
            {
                t.localIdentifier = localIdentifier;
            }

            foreach (var t in achievements)
            {
                t.localIdentifier = localIdentifier;
            }

            questStates.Add(localIdentifier, new QuestsContainer());
        }

        public Quest GetActiveQuestByID(int questID)
        {
            return GetActiveQuestByID(questID, localIdentifier);
        }

        public Quest GetActiveQuestByID(int questID, ILocalIdentifier localIdentifier)
        {
            return questStates[localIdentifier].activeQuests.FirstOrDefault(o => o.ID == questID);
        }

        public Dictionary<ILocalIdentifier, QuestsContainer> GetAllQuestStates()
        {
            return questStates;
        }

        public QuestsContainer GetQuestStates()
        {
            return GetQuestStates(localIdentifier);
        }

        public QuestsContainer GetQuestStates(ILocalIdentifier localIdentifier)
        {
            Assert.IsNotNull(localIdentifier, "Local identifier is null. Quest (most likely) doesn't exist in current database.");
            if (questStates.ContainsKey(localIdentifier) == false)
            {
                DevdogLogger.LogError("No quest states found for localIdentifier: " + localIdentifier.ToString());
                return null;
            }

            return questStates[localIdentifier];
        }
        
        public virtual bool HasActiveQuest(Quest quest)
        {
            Assert.IsNotNull(quest.localIdentifier, "Quest local identifier is null. Quest (most likely) doesn't exist in current database.");
            return questStates[quest.localIdentifier].activeQuests.Contains(quest);
        }

        public virtual bool HasCompletedQuest(Quest quest)
        {
            Assert.IsNotNull(quest.localIdentifier, "Quest local identifier is null. Quest (most likely) doesn't exist in current database.");
            return questStates[quest.localIdentifier].completedQuests.Contains(quest);
        }


        public void NotifyQuestTaskReachedTimeLimit(Task task, Quest quest)
        {
            if (OnQuestTaskReachedTimeLimit != null)
            {
                OnQuestTaskReachedTimeLimit(task, quest);
            }   
        }

        public virtual void NotifyQuestTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Quest quest)
        {
            if (OnQuestTaskStatusChanged != null)
            {
                OnQuestTaskStatusChanged(before, task, quest);
            }
        }

        public virtual void NotifyQuestTaskProgressChanged(float before, Task task, Quest quest)
        {
            if (OnQuestTaskProgressChanged != null)
            {
                OnQuestTaskProgressChanged(before, task, quest);
            }
        }

        public virtual void NotifyQuestStatusChanged(QuestStatus before, Quest quest)
        {
            Assert.IsNotNull(quest.localIdentifier, "Quest local identifier is null. Quest (most likely) doesn't exist in current database.");
            switch (quest.status)
            {
                case QuestStatus.InActive:
                case QuestStatus.Cancelled:
                    questStates[quest.localIdentifier].activeQuests.Remove(quest);
                    break;
                case QuestStatus.Active:
                    questStates[quest.localIdentifier].activeQuests.Add(quest);
                    break;
                case QuestStatus.Completed:
                    questStates[quest.localIdentifier].activeQuests.Remove(quest);
                    questStates[quest.localIdentifier].completedQuests.Add(quest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (OnQuestStatusChanged != null)
            {
                OnQuestStatusChanged(before, quest);
            }
        }


        public void NotifyAchievementTaskReachedTimeLimit(Task task, Achievement achievement)
        {
            if (OnAchievementTaskReachedTimeLimit != null)
            {
                OnAchievementTaskReachedTimeLimit(task, achievement);
            }
        }

        public virtual void NotifyAchievementTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Achievement achievement)
        {
            if (OnAchievementTaskStatusChanged != null)
            {
                OnAchievementTaskStatusChanged(before, task, achievement);
            }
        }

        public virtual void NotifyAchievementTaskProgressChanged(float before, Task task, Achievement achievement)
        {
            if (OnAchievementTaskProgressChanged != null)
            {
                OnAchievementTaskProgressChanged(before, task, achievement);
            }
        }

        public virtual void NotifyAchievementStatusChanged(QuestStatus before, Achievement achievement)
        {
            if (OnAchievementStatusChanged != null)
            {
                OnAchievementStatusChanged(before, achievement);
            }

            if (achievement.status == QuestStatus.Active)
            {
                GetQuestStates(achievement.localIdentifier).achievements.Add(achievement);
            }
        }

        public virtual void Reset()
        {
            questStates.Clear();
            Init();
        }
    }
}