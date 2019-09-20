using System;
using System.Reflection;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Enables / disables an object when the quest status is set.
    /// </summary>
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Quest Object Affectors/Component Enabler")]
    public sealed class QuestStatusComponentEnabler : QuestStatusObjectBase
    {
        public enum Action
        {
            Enable,
            Disable,
        }

        [Header("Configuration")]
        public Action action = Action.Enable;
        public Component[] components = new Component[0];

        protected override void OnStatusChangedCorrect(Quest self)
        {
            foreach (var comp in components)
            {
                var prop = comp.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    prop.SetValue(comp, action == Action.Enable, null);
                }
            }
        }

        protected override void OnStatusChangedInCorrect(Quest self)
        {
            foreach (var comp in components)
            {
                var prop = comp.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    prop.SetValue(comp, action == Action.Disable, null);
                }
            }
        }
    }
}
