using UnityEngine.Events;

namespace Devdog.QuestSystemPro.UI
{
    public class AchievementStatusBlockUI : QuestStatusBlockUIBase<Achievement>, IAchievementStatusBlockUI
    {
        [System.Serializable]
        public class OnAction : UnityEvent<Quest>
        { }


        public QuestStatusBlockUI.OnAction onEnable;
        public QuestStatusBlockUI.OnAction onDisable;

        protected override void SetActive(bool b, Achievement achievement)
        {
            base.SetActive(b, achievement);
            if (b)
            {
                onEnable.Invoke(achievement);
            }
            else
            {
                onDisable.Invoke(achievement);
            }
        }
    }
}