using UnityEngine.Events;

namespace Devdog.QuestSystemPro.UI
{
    public class QuestStatusBlockUI : QuestStatusBlockUIBase<Quest>, IQuestStatusBlockUI
    {
        [System.Serializable]
        public class OnAction : UnityEvent<Quest>
        { }


        public OnAction onEnable;
        public OnAction onDisable;

        protected override void SetActive(bool b, Quest quest)
        {
            base.SetActive(b, quest);
            if (b)
            {
                onEnable.Invoke(quest);
            }
            else
            {
                onDisable.Invoke(quest);
            }
        }
    }
}