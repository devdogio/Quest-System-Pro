using System;
using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public class QuestSidebarUI : MonoBehaviour
    {
        public TaskFilter tasksFilter = TaskFilter.ActiveAndCompleted;
        public bool showOverAchievement = false;
        //        public int showMaxQuests = 5; // TODO: Integrate feature: max amount of active sidebar quests.

        [Header("Prefabs")]
        public QuestProgressRowUI progressRowUIPrefab;

        [Header("UI References")]
        public RectTransform questsContainer;

        [Header("Progress value")]
        protected Dictionary<Quest, QuestProgressRowUI> uiCache = new Dictionary<Quest, QuestProgressRowUI>(); 

        protected virtual void Awake()
        { }

        protected virtual void Start()
        { }

        public virtual bool ContainsQuest(Quest quest)
        {
            return uiCache.ContainsKey(quest);
        }

        public virtual void AddQuest(Quest quest)
        {
            uiCache[quest] = CreateUIRowInstance();
            uiCache[quest].Repaint(quest);

            quest.OnStatusChanged += OnQuestStatusChanged;
            quest.OnTaskStatusChanged += OnQuestTaskStatusChanged;
            quest.OnTaskProgressChanged += OnQuestTaskProgressChanged;
        }

        public virtual void RemoveQuest(Quest quest)
        {
            if (ContainsQuest(quest))
            {
                var a = uiCache[quest];
                uiCache.Remove(quest);

                Destroy(a.gameObject);

                quest.OnStatusChanged -= OnQuestStatusChanged;
                quest.OnTaskStatusChanged -= OnQuestTaskStatusChanged;
                quest.OnTaskProgressChanged -= OnQuestTaskProgressChanged;
            }
        }

        private void OnQuestStatusChanged(QuestStatus before, Quest quest)
        {
            switch (quest.status)
            {
                case QuestStatus.InActive:
                case QuestStatus.Completed:
                case QuestStatus.Cancelled:

                    RemoveQuest(quest);
                    break;
                case QuestStatus.Active:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnQuestTaskStatusChanged(TaskStatus before, Task task, Quest quest)
        {
            Repaint(quest);
        }

        private void OnQuestTaskProgressChanged(float taskProgressBefore, Task task, Quest quest)
        {
            Repaint(quest);
        }

        private QuestProgressRowUI CreateUIRowInstance()
        {
            var inst = Instantiate<QuestProgressRowUI>(progressRowUIPrefab);
            inst.showTaskRewards = false;
            inst.showTasksFilter = tasksFilter;

            inst.transform.SetParent(questsContainer);
            UIUtility.ResetTransform(inst.transform);

            return inst;
        }

        public void RepaintAll()
        {
            foreach (var kvp in uiCache)
            {
                Repaint(kvp.Key);
            }
        }

        public virtual void Repaint(Quest quest)
        {
            if (ContainsQuest(quest) == false)
            {
                AddQuest(quest);
                return;
            }

            uiCache[quest].Repaint(quest);
        }
    }
}