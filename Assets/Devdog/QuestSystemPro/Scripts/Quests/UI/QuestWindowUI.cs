using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(UIWindow))]
    public class QuestWindowUI : QuestWindowUIBase
    {
        [System.Serializable]
        public class OnQuestActionCallback : UnityEvent<Quest>
        {
            
        }


        [Header("Prefabs")]
        public TaskProgressRowUI taskProgressPrefab;

        [Header("References")]
        public Text questName;
        public Text questDescription;
        public Text questActivateConditions;
        public RectTransform rewardsContainer;
        public RectTransform tasksContainer;

        [Header("Buttons & Events")]
        public Button acceptButton;
        public OnQuestActionCallback acceptCallback;

        public Button declineButton;
        public OnQuestActionCallback declineCallback;

        public Button cancelButton;
        public OnQuestActionCallback cancelCallback;

        public Button completeButton;
        public OnQuestActionCallback completeCallback;


        protected override void Awake()
        {
            base.Awake();

            if(acceptButton != null)
                acceptButton.onClick.AddListener(OnClickAccept);

            if(declineButton != null)
                declineButton.onClick.AddListener(OnClickDecline);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnClickCancel);

            if (completeButton != null)
                completeButton.onClick.AddListener(OnClickComplete);
        }

        public override void Repaint(Quest quest)
        {
            selectedQuest = quest;
            Assert.IsNotNull(selectedQuest, "Current quest is not set, but requesting repaint of window.");

            Set(questName, selectedQuest.name.message);
            Set(questDescription, selectedQuest.description.message);
            Set(questActivateConditions, selectedQuest.CanActivate().ToString());

            if (tasksContainer != null)
            {
                RepaintQuestTasks(selectedQuest);
            }

            if (rewardsContainer != null)
            {
                RepaintQuestRewards(selectedQuest);
            }

            RepaintStatusBlocks();
            window.Show();
        }

        protected virtual void RepaintQuestRewards(Quest quest)
        {
            foreach (Transform child in rewardsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var rewardGiver in quest.rewardGivers)
            {
                if (rewardGiver.rewardUIPrefab == null)
                {
                    continue;
                }

                var inst = CreateUIElement(rewardGiver.rewardUIPrefab, rewardsContainer);
                inst.Repaint(rewardGiver, quest);
            }
        }

        protected virtual void RepaintQuestTasks(Quest quest)
        {
            foreach (Transform child in tasksContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var task in quest.tasks)
            {
                var inst = CreateUIElement(taskProgressPrefab, tasksContainer);
                inst.Repaint(task);
            }
        }

        protected virtual void RepaintStatusBlocks()
        {
            QuestUIUtility.RepaintQuestUIRepaintableChildren(transform, selectedQuest);
        }

        protected virtual void OnClickAccept()
        {
            selectedQuest.Activate();
            acceptCallback.Invoke(selectedQuest);
            RepaintStatusBlocks();
        }

        protected virtual void OnClickDecline()
        {
            selectedQuest.Decline();
            declineCallback.Invoke(selectedQuest);
            RepaintStatusBlocks();
        }

        protected virtual void OnClickCancel()
        {
            selectedQuest.Cancel();
            cancelCallback.Invoke(selectedQuest);
            RepaintStatusBlocks();
        }

        protected virtual void OnClickComplete()
        {
            selectedQuest.CompleteAndGiveRewards();
            completeCallback.Invoke(selectedQuest);
            RepaintStatusBlocks();
        }
    }
}