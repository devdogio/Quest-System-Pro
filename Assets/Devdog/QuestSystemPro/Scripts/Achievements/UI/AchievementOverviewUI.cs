using System.Collections.Generic;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UIWindow))]
    public class AchievementOverviewUI : MonoBehaviour
    {
        [Header("UI references")]
        public RectTransform achievementsContainer;

        [Header("Prefabs")]
        public ClaimableAchievementUI achievementUIPrefab;

        protected UIWindow window;
        protected Dictionary<Achievement, ClaimableAchievementUI> uiElements = new Dictionary<Achievement, ClaimableAchievementUI>(); 

        protected virtual void Awake()
        {
            window = GetComponent<UIWindow>();
        }

        protected virtual void Start()
        {
            FillUI();
            QuestManager.instance.OnAchievementTaskProgressChanged += OnAchievementTaskProgressChanged;
        }

        protected virtual void OnAchievementTaskProgressChanged(float taskProgressBefore, Task task, Achievement achievement)
        {
            if (uiElements.ContainsKey(achievement))
            {
                uiElements[achievement].Repaint(taskProgressBefore, task, achievement);
            }
            else
            {
                DevdogLogger.LogWarning("Couldn't repaint UI element for achievement " + achievement.ID + " - Not found in dict UI lookup.");
            }
        }

        protected virtual void FillUI()
        {
            uiElements.Clear();
            foreach (var achievement in QuestManager.instance.achievements)
            {
                var ui = CreateUIElement(achievement);
                ui.Repaint(achievement);

                uiElements.Add(achievement, ui);
            }
        }

        protected virtual ClaimableAchievementUI CreateUIElement(Achievement achievement)
        {
            var ui = Instantiate<ClaimableAchievementUI>(achievementUIPrefab);
            ui.transform.SetParent(achievementsContainer);
            ui.transform.position = Vector3.zero;
            ui.transform.localScale = Vector3.one;

            return ui;
        }
    }
}