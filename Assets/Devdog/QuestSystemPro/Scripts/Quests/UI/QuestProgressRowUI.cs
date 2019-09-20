using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    /// <summary>
    /// Represents a single quest (row) and show's it's progress.
    /// </summary>
    public class QuestProgressRowUI : MonoBehaviour
    {
        [Header("Options")]
//        public bool onlyShowActiveTasks = true;
        public bool showTaskRewards = true;
        public TaskFilter showTasksFilter = TaskFilter.Active;

        [Header("UI Elements")]
        public Text title;
        public Text description;
        public RectTransform tasksContainer;
        public RectTransform rewardsContainer;

//        [Header("Visuals & Audio")]
//        public AnimationClip showAnimation;
//        public AnimationClip hideAnimation;
//        public AudioClip audioClip;

        protected Dictionary<Task, TaskProgressRowUI> taskUICache = new Dictionary<Task, TaskProgressRowUI>();
        protected Dictionary<IRewardGiver, RewardRowUI> rewardGiverUICache = new Dictionary<IRewardGiver, RewardRowUI>();

        public virtual void Repaint(Quest quest)
        {
            UpdateTaskProgressRowsUI(quest);
            if (rewardsContainer != null)
            {
                UpdateRewardsRowsUI(quest);
            }

            if (title != null)
                title.text = quest.name.message;

            if(description != null)
                description.text = quest.description.message;

            QuestUIUtility.RepaintQuestUIRepaintableChildren(transform, quest);
        }

        private void UpdateRewardsRowsUI(Quest quest)
        {
            if (showTaskRewards == false)
            {
                if (rewardsContainer != null)
                {
                    rewardsContainer.gameObject.SetActive(false);
                }

                return;
            }


            rewardsContainer.gameObject.SetActive(true);

            // TODO: Pool this.
            foreach (var kvp in rewardGiverUICache)
            {
                Destroy(kvp.Value.gameObject);
            }

            rewardGiverUICache.Clear();
            foreach (var rewardGiver in quest.rewardGivers)
            {
                if (rewardGiver.rewardUIPrefab == null)
                {
                    continue;
                }

                var ui = CreateRewardRow(rewardGiver);
                ui.Repaint(rewardGiver, quest);
                rewardGiverUICache.Add(rewardGiver, ui);
            }
        }

        protected virtual void UpdateTaskProgressRowsUI(Quest quest)
        {
            // TODO: Pool this.
            foreach (var taskRowUI in taskUICache)
            {
                Destroy(taskRowUI.Value.gameObject);
            }

            taskUICache.Clear();

            var tasks = quest.GetTasks(showTasksFilter);
            foreach (var activeTask in tasks)
            {
                if (activeTask.taskUIPrefab == null)
                {
                    continue;
                }

                var ui = CreateTaskRowUI(activeTask);
                ui.Repaint(activeTask);
                taskUICache.Add(activeTask, ui);
            }
        }

        protected virtual TaskProgressRowUI CreateTaskRowUI(Task task)
        {
            var inst = Instantiate<TaskProgressRowUI>(task.taskUIPrefab);
            inst.transform.SetParent(tasksContainer);
            UIUtility.ResetTransform(inst.transform);

            return inst;
        }

        protected virtual RewardRowUI CreateRewardRow(IRewardGiver rewardGiver)
        {
            var inst = Instantiate<RewardRowUI>(rewardGiver.rewardUIPrefab);
            inst.transform.SetParent(rewardsContainer);
            UIUtility.ResetTransform(inst.transform);

            return inst;
        }
    }
}