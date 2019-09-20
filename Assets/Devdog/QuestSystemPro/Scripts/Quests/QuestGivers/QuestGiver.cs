using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Devdog.QuestSystemPro
{
    public class QuestGiver : MonoBehaviour, IQuestGiver, ITriggerWindowContainer, ITriggerCallbacks
    {
        [SerializeField]
        [FormerlySerializedAs("quests")]
        private Quest[] _quests = new Quest[0];
        public Quest[] quests
        {
            get { return _quests; }
            set { _quests = value; }
        }
        
        public UIWindow window
        {
            get { return QuestManager.instance.questWindowUI.window; }
        }

        protected virtual void Awake()
        {

        }

        public virtual bool OnTriggerUsed(Player player)
        {
            Use();
            return false;
        }

        public virtual bool OnTriggerUnUsed(Player player)
        {
            UnUse();
            return false;
        }

        public Quest[] GetAvailableQuests(ILocalIdentifier identifier)
        {
            // TODO: For the server: Get all quests that identifier can accept.
            return quests.Where(o => o.CanActivate().status).ToArray();
        }

        public virtual void Use()
        {
            QuestManager.instance.currentQuestGiver = this;

            var q = quests.FirstOrDefault(o => o != null && (o.CanActivate().status || o.status == QuestStatus.Active));
            if (q != null)
            {
                QuestManager.instance.questWindowUI.Repaint(q);
            }
        }

        public virtual void UnUse()
        {
            QuestManager.instance.questWindowUI.window.Hide();

            if (ReferenceEquals(QuestManager.instance.currentQuestGiver, this))
            {
                QuestManager.instance.currentQuestGiver = null;
            }
        }
    }
}
