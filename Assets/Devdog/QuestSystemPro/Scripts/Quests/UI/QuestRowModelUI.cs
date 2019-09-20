using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    [System.Serializable]
    public class QuestRowModelUI
    {
        public Image icon;

        public Text ownerName;
        public Text description;
        public Text statusMessage;

        public UIShowValue progress;


        public virtual void Repaint(Quest quest)
        {
            if (icon != null)
            {
                icon.sprite = quest.icon;
            }

            SetText(ownerName, quest.name.message);
            SetText(description, quest.description.message);
            SetText(statusMessage, string.Empty);
        }

        public virtual void Repaint(Task task)
        {
            if (icon != null)
            {
                icon.sprite = task.owner.icon;
            }

            SetText(ownerName, task.owner.name.message);
            SetText(description, task.description.message);
            SetText(statusMessage, task.GetStatusMessage());
        }

        protected void SetText(Text text, string msg)
        {
            if (text != null)
            {
                text.text = msg;
            }
        }
    }
}
