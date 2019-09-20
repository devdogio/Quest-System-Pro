using System.Collections;
using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UIWindow))]
    public class QuestTaskProgressUI : MonoBehaviour
    {
        public QuestRowModelUI uiModel = new QuestRowModelUI();
        public bool showOverAchievement = false;

        [Header("Progress value")]
        public bool hideAfterSeconds = true;
        public float showForSeconds = 5f;
        public float interpSpeed = 1f;
        public AnimationCurve interpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        protected Queue<TaskPreviousProgressPair> queue = new Queue<TaskPreviousProgressPair>();
        protected UIWindow window;

        protected virtual void Awake()
        {
            window = GetComponent<UIWindow>();
        }

        protected virtual void Start()
        {
            RegisterListener();
            StartCoroutine(ShowProgress());
        }

        protected virtual void OnDestroy()
        {
            UnRegisterListener();
        }

        protected virtual void RegisterListener()
        {
            QuestManager.instance.OnQuestTaskProgressChanged += OnTaskProgressChanged;
        }

        protected virtual void UnRegisterListener()
        {
            if(QuestManager.instance != null)
                QuestManager.instance.OnQuestTaskProgressChanged -= OnTaskProgressChanged;
        }

        protected virtual void OnTaskProgressChanged(float taskProgressBefore, Task task, Quest quest)
        {
            if (task.progress <= task.progressCap || showOverAchievement)
            {
                queue.Enqueue(new TaskPreviousProgressPair(taskProgressBefore, task));
            }
        }

        private IEnumerator ShowProgress()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    bool alreadyVisible = window.isVisible;
                    var quest = queue.Dequeue();
                    window.Show();

                    if (alreadyVisible)
                    {
                        window.PlayAudio(window.showAudioClip);
                    }

                    uiModel.Repaint(quest.task);
                    yield return StartCoroutine(InterpolateValueTo(quest));

                    float timer = 0f;
                    while (timer < showForSeconds)
                    {
                        if (queue.Count != 0)
                        {
                            // An item got added while we were waiting, stop waiting and show the item now.
                            break;
                        }

                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    if (hideAfterSeconds)
                    {
                        window.Hide();
                    }
                }

                yield return null;
            }
        }

        private IEnumerator InterpolateValueTo(TaskPreviousProgressPair to)
        {
            float timer = 0f;
            while (timer < interpSpeed)
            {
                var fromNormalized = to.taskProgressBefore / to.task.progressCap;
                var startVal = to.task.progressNormalized - fromNormalized;
                var nValue = interpCurve.Evaluate(timer / interpSpeed) * startVal;
                uiModel.progress.Repaint(fromNormalized + nValue, 1f);

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}