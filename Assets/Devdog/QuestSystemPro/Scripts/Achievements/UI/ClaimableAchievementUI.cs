using System.Collections;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    public class ClaimableAchievementUI : MonoBehaviour
    {
        public QuestRowModelUI uiModel = new QuestRowModelUI();
        public bool showOverAchievement = false;

        [Header("Progress value")]
        public float interpSpeed = 1f;
        public AnimationCurve interpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        protected Queue<TaskPreviousProgressPair> queue = new Queue<TaskPreviousProgressPair>();
        protected Button button;
        protected Achievement currentAchievement;

        private Coroutine _coroutine;

        protected virtual void Awake()
        {
            button = GetComponentInChildren<Button>();

            if (button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnButtonClicked()
        {
            if (currentAchievement != null && currentAchievement.CanComplete().status)
            {
                currentAchievement.CompleteAndGiveRewards();
                Repaint(currentAchievement);
            }
        }

        public void Repaint(Achievement achievement)
        {
            Repaint(0f, achievement.tasks.FirstOrDefault(), achievement);
        }

        public virtual void Repaint(float progressBefore, Task task, Achievement achievement)
        {
            currentAchievement = achievement;
            uiModel.Repaint(task);

            RepaintStatusBlocks(achievement);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(InterpolateValueTo(progressBefore, task, achievement));
        }

        protected void RepaintStatusBlocks(Achievement achievement)
        {
            QuestUIUtility.RepaintAchievementUIRepaintableChildren(transform, achievement);
        }

        private IEnumerator InterpolateValueTo(float taskProgressBefore, Task task, Achievement achievement)
        {
            float timer = 0f;
            while (timer < interpSpeed)
            {
                timer += Time.deltaTime;

                var from = taskProgressBefore / task.progressCap;
                var nValue = interpCurve.Evaluate(timer) * (task.progressNormalized - from);
                uiModel.progress.Repaint(from + nValue, 1f);

                yield return null;
            }
        }
    }
}