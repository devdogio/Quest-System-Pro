using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Enables / disables an object when the quest status is set.
    /// </summary>
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Quest Object Affectors/Object Enabler")]
    public sealed class QuestStatusObjectEnabler : QuestStatusObjectBase
    {
        public enum Action
        {
            Enable,
            Disable,
        }

        [Header("Configuration")]
        public Action action = Action.Enable;

        protected override void OnStatusChangedCorrect(Quest self)
        {
            gameObject.SetActive(action == Action.Enable);
        }

        protected override void OnStatusChangedInCorrect(Quest self)
        {
            gameObject.SetActive(action == Action.Disable);
        }
    }
}
