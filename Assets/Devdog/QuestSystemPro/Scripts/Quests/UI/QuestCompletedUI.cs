using System.Collections;
using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UIWindow))]
    public class QuestCompletedUI : MonoBehaviour
    {
        public QuestRowModelUI uiModel = new QuestRowModelUI();

        [Header("Audio & Visuals")]
        public float showForSeconds = 2f;

        protected UIWindow window;
        protected Coroutine coroutine;
        private WaitForSeconds _wait;

        protected virtual void Awake()
        {
            _wait = new WaitForSeconds(showForSeconds);
            window = GetComponent<UIWindow>();
        }

        protected virtual void Start()
        {
            RegisterListener();
        }

        protected virtual void OnDestroy()
        {
            UnRegisterListener();
        }

        protected virtual void RegisterListener()
        {
            QuestManager.instance.OnQuestStatusChanged += OnStatusChanged;
        }

        protected virtual void UnRegisterListener()
        {
            if(QuestManager.instance != null)
                QuestManager.instance.OnQuestStatusChanged -= OnStatusChanged;
        }

        protected void OnStatusChanged(QuestStatus before, Quest self)
        {
            if (self.status == QuestStatus.Completed)
            {
                uiModel.Repaint(self);
                window.Show();

                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }

                coroutine = StartCoroutine(_WaitAndHideWindow());
            }
        }

        private IEnumerator _WaitAndHideWindow()
        {
            yield return _wait;
            window.Hide();
        }
    }
}