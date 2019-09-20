using System.Collections;
using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public class AchievementCompletedUI : QuestCompletedUI
    {
        protected override void RegisterListener()
        {
            QuestManager.instance.OnAchievementStatusChanged += OnAchievementStatusChanged;
        }

        protected override void UnRegisterListener()
        {
            if(QuestManager.instance != null)
                QuestManager.instance.OnAchievementStatusChanged -= OnAchievementStatusChanged;
        }

        private void OnAchievementStatusChanged(QuestStatus before, Achievement self)
        {
            OnStatusChanged(before, self);
        }
    }
}