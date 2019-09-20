using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.Events;

namespace Devdog.QuestSystemPro.UI
{
    public abstract class QuestStatusBlockUIBase<T> : MonoBehaviour where T : Quest
    {
        public QuestStatus[] status = new QuestStatus[0];

        public bool onlyShowWhenCompletable;
        public bool onlyShowWhenNotCompletable;

        protected virtual void Awake()
        {
            gameObject.SetActive(false); // Disable by default and wait for Repaint() callback.
        }

        public virtual void Repaint(T quest)
        {
            if (status.Contains(quest.status))
            {
                if (onlyShowWhenCompletable)
                {
                    if (quest.CanComplete() == false)
                    {
                        SetActive(false, quest);
                        return;
                    }   
                }
                else if (onlyShowWhenNotCompletable)
                {
                    if (quest.CanComplete().status)
                    {
                        SetActive(false, quest);
                        return;
                    }
                }

                SetActive(true, quest);
            }
            else
            {
                SetActive(false, quest);
            }
        }

        protected virtual void SetActive(bool b, T quest)
        {
            gameObject.SetActive(b);
        }
    }
}