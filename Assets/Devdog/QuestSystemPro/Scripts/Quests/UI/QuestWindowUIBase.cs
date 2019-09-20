using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(UIWindow))]
    public abstract class QuestWindowUIBase : MonoBehaviour
    {
        public UIWindow window { get; protected set; }

        private Quest _selectedQuest;
        public Quest selectedQuest
        {
            get { return _selectedQuest; }
            protected set
            {
                if (_selectedQuest != null)
                {
                    _selectedQuest.OnTaskStatusChanged -= OnSelectedQuestTaskStatusChanged;
                    _selectedQuest.OnTaskProgressChanged -= OnSelectedQuestTaskProgressChanged;
                    _selectedQuest.OnStatusChanged -= OnSelectedQuestStatusChanged;
                }

                _selectedQuest = value;
                if (_selectedQuest != null)
                {
                    _selectedQuest.OnTaskStatusChanged += OnSelectedQuestTaskStatusChanged;
                    _selectedQuest.OnTaskProgressChanged += OnSelectedQuestTaskProgressChanged;
                    _selectedQuest.OnStatusChanged += OnSelectedQuestStatusChanged;
                }
            }
        }
        
        protected virtual void Awake()
        {
            window = GetComponent<UIWindow>();
            window.OnShow += WindowOnShow;
        }

        protected virtual void WindowOnShow()
        {
            if (selectedQuest != null)
            {
                Repaint(selectedQuest);
            }
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnSelectedQuestStatusChanged(QuestStatus before, Quest quest)
        {
            if (window.isVisible)
            {
                Repaint(quest);
            }
        }

        protected virtual void OnSelectedQuestTaskProgressChanged(float taskProgressBefore, Task task, Quest quest)
        {
            if (window.isVisible)
            {
                Repaint(quest);
            }
        }

        protected virtual void OnSelectedQuestTaskStatusChanged(TaskStatus before, Task task, Quest quest)
        {
            if (window.isVisible)
            {
                Repaint(quest);
            }
        }

        public abstract void Repaint(Quest quest);

        protected virtual T CreateUIElement<T>(T prefab, RectTransform parent) where T : MonoBehaviour
        {
            var inst = Instantiate<T>(prefab);
            inst.transform.SetParent(parent);
            UIUtility.ResetTransform(inst.transform);

            return inst;
        }

        protected void Set(Text text, string msg)
        {
            if (text != null)
            {
                text.text = msg;
            }
        }
        protected void Set(Image image, Sprite sprite)
        {
            if (image != null)
            {
                image.sprite = sprite;
            }
        }
    }
}