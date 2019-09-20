using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    public class TaskProgressRowUI : MonoBehaviour
    {
        [Header("Options")]
        public Color activeColor = Color.white;
        public Color completedColor = Color.gray;
        public Color inActiveColor = Color.gray;

        public bool hideProgressWhenCompleted = true;
        public bool hideProgressWhenInActive = true;

        [Header("UI Elements")]
        public Text taskName;
        public Text taskDescription;
        public Image taskIcon;
        public Text statusMessage;
        public UIShowValue progress;
        public UIShowValue timer;

        [Header("Interpolation")]
        public float interpSpeed = 1f;
        public AnimationCurve interpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


        protected Task currentTask;
        protected float previousTaskProgressNormalized = 0f;


        protected virtual void Update()
        {
            if (currentTask != null && currentTask.useTimeLimit && currentTask.status == TaskStatus.Active)
            {
                timer.Repaint((float)currentTask.timeSinceStartInSeconds, currentTask.timeLimitInSeconds);
            }
        }

        public virtual void Repaint(Task task)
        {
            currentTask = task;
            var statusColor = GetStatusColor(task);

            if (taskName != null)
            {
                taskName.text = task.key;
                taskName.color = statusColor;
            }

            if(taskDescription != null) 
            {
                taskDescription.text = task.description.message;
                taskDescription.color = statusColor;
            }

            if (taskIcon != null)
            {
                taskIcon.gameObject.SetActive(task.icon != null);

                taskIcon.sprite = task.icon;
                taskIcon.color = statusColor;
            }

            if (statusMessage != null)
            {
                statusMessage.text = task.GetStatusMessage();
                statusMessage.color = statusColor;
            }

            if (task.useTimeLimit == false)
            {
                timer.HideAll();
            }

            if ((hideProgressWhenInActive && task.owner.GetActiveTasks().Contains(task) == false) ||
                (hideProgressWhenCompleted && task.isCompleted))
            {
                progress.HideAll();
            }
            else
            {
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(_InterpolateProgress(task));
                }
            }
        }

        protected Color GetStatusColor(Task task)
        {
            if (task.isCompleted)
            {
                return completedColor;
            }

            if (task.owner.GetActiveTasks().Contains(task))
            {
                return activeColor;
            }

            return inActiveColor;
        }

        protected virtual IEnumerator _InterpolateProgress(Task task)
        {
            previousTaskProgressNormalized = task.progressNormalized;

            float time = 0f;
            while (time < interpSpeed)
            {
                time += Time.deltaTime;

                var nValue = interpCurve.Evaluate(time) * (task.progressNormalized - previousTaskProgressNormalized);
                progress.Repaint(previousTaskProgressNormalized + nValue, 1f);

                yield return null;
            }
        }
    }
}