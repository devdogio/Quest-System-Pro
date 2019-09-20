using System.Collections;
using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UIWindow))]
    public class AchievementTaskProgressUI : QuestTaskProgressUI
    {
        protected override void RegisterListener()
        {
            QuestManager.instance.OnAchievementTaskProgressChanged += OnAchievementTaskProgressChanged;
        }

        protected override void UnRegisterListener()
        {
            if (QuestManager.instance != null)
                QuestManager.instance.OnAchievementTaskProgressChanged -= OnAchievementTaskProgressChanged;
        }

        protected virtual void OnAchievementTaskProgressChanged(float taskProgressBefore, Task task, Achievement achievement)
        {
            OnTaskProgressChanged(taskProgressBefore, task, achievement);
        }
    }
}