using System.Collections.Generic;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public static class QuestUIUtility
    {
        private static List<IQuestStatusBlockUI> _questUIRepaintables = new List<IQuestStatusBlockUI>();
        private static List<IAchievementStatusBlockUI> _achievementUIRepaintables = new List<IAchievementStatusBlockUI>();


        public static void RepaintQuestUIRepaintableChildren(Transform transform, Quest quest)
        {
            _questUIRepaintables.Clear();
            transform.GetComponentsInChildren<IQuestStatusBlockUI>(true, _questUIRepaintables);
            foreach (var repaintable in _questUIRepaintables)
            {
                repaintable.Repaint(quest);
            }
        }
        
        public static void RepaintAchievementUIRepaintableChildren(Transform transform, Achievement achievement)
        {
            _achievementUIRepaintables.Clear();
            transform.GetComponentsInChildren<IAchievementStatusBlockUI>(true, _achievementUIRepaintables);
            foreach (var repaintable in _achievementUIRepaintables)
            {
                repaintable.Repaint(achievement);
            }
        }
    }
}
