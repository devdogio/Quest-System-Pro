using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public class QuestSpawner : SpawnerBase
    {
        [Header("Quest")]
        public QuestStatusDecorator questStatus;

        [Header("Misc")]
        public bool destroyObjectsOnQuestEnd = true;

        protected void OnEnable()
        {
            questStatus.RegisterCallbacks(OnQuestStatusChanged, OnQuestTaskStatusChanged);
        }

        protected void OnDisable()
        {
            questStatus.UnRegisterCallbacks(OnQuestStatusChanged, OnQuestTaskStatusChanged);
        }

        protected virtual void OnQuestTaskStatusChanged(TaskStatus before, TaskStatus after, Task task)
        {
            if (task.status == questStatus.taskStatus)
            {
                Spawn();
            }
            else if (before == questStatus.taskStatus && task.status != questStatus.taskStatus)
            {
                // Became inactive, despawn all
                DestroyAllSpawnedObjects();
                StopAllCoroutines();
            }
        }

        protected virtual void OnQuestStatusChanged(QuestStatus before, Quest q)
        {
            if (q.status == questStatus.questStatus)
            {
                Spawn();
            }
            else if (before == questStatus.questStatus && q.status != questStatus.questStatus)
            {
                // Became inactive, despawn all
                DestroyAllSpawnedObjects();
                StopAllCoroutines();
            }
        }

        protected override void OnBecameRelevant()
        {
            if (questStatus.useTaskStatus)
            {
                var task = questStatus.quest.GetTask(questStatus.taskName);
                if (task.status == questStatus.taskStatus)
                {
                    base.OnBecameRelevant();
                }
            }
            else if (questStatus.quest.status == questStatus.questStatus)
            {
                base.OnBecameRelevant();
            }
        }

        protected override void OnBecameIrrelevant()
        {
            if (questStatus.useTaskStatus)
            {
                var task = questStatus.quest.GetTask(questStatus.taskName);
                if (task.status == questStatus.taskStatus)
                {
                    base.OnBecameIrrelevant();
                }
            }
            else if (questStatus.quest.status == questStatus.questStatus)
            {
                base.OnBecameIrrelevant();
            }
        }
    }
}
