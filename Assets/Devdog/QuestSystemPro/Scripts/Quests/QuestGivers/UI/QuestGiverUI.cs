using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public class QuestGiverUI : MonoBehaviour
    {
        [Header("UI")]
        public SpriteRenderer spriteRenderer;
        public Sprite availableQuest;
        public Sprite completableQuest;


        private IQuestGiver _questGiver;

        protected virtual void Start()
        {
            _questGiver = GetComponent<IQuestGiver>();
            foreach (var quest in _questGiver.quests)
            {
                quest.OnTaskProgressChanged += OnQuestTaskProgressChanged;
                quest.OnStatusChanged += OnQuestStatusChanged;

                OnQuestChanged(quest); // Invoke first time on start.
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var quest in _questGiver.quests)
            {
                quest.OnTaskProgressChanged -= OnQuestTaskProgressChanged;
                quest.OnStatusChanged -= OnQuestStatusChanged;
            }
        }

        protected virtual void OnQuestStatusChanged(QuestStatus before, Quest quest)
        {
            OnQuestChanged(quest);
        }

        protected virtual void OnQuestTaskProgressChanged(float before, Task task, Quest quest)
        {
            OnQuestChanged(quest);
        }

        protected virtual void OnQuestChanged(Quest quest)
        {
            if (_questGiver.quests.Any(o => o.CanComplete().status))
            {
                Show(completableQuest);
            }
            else if (_questGiver.quests.Any(o => o.CanActivate().status && o.status != QuestStatus.Active))
            {
                Show(availableQuest);
            }
            else
            {
                Show(null);
            }
        }

        protected virtual void Show(Sprite sprite)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
            }
        }
    }
}
