using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public class QuestWindowUIHelper : MonoBehaviour
    {
        private QuestWindowUI _questWindow;

        protected virtual void Awake()
        {
            _questWindow = GetComponentInParent<QuestWindowUI>();
        }

        public void ActivateCurrentQuest()
        {
            if (_questWindow.selectedQuest != null)
            {
                _questWindow.selectedQuest.Activate();
            }
        }

        public void DeclineCurrentQuest()
        {
            if (_questWindow.selectedQuest != null)
            {
                _questWindow.selectedQuest.Decline();
            }
        }

        public void CancelCurrentQuest()
        {
            if (_questWindow.selectedQuest != null)
            {
                _questWindow.selectedQuest.Cancel();
            }
        }

        public void CompleteCurrentQuest()
        {
            if (_questWindow.selectedQuest != null)
            {
                _questWindow.selectedQuest.CompleteAndGiveRewards();
            }
        }
    }
}