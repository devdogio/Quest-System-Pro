using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Destroys an object when the quest status is set.
    /// </summary>
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Quest Object Affectors/Object Destructor")]
    public sealed class QuestStatusObjectDestructor : QuestStatusObjectBase
    {
        protected override void OnStatusChangedCorrect(Quest self)
        {
            Destroy(gameObject);
        }
    }
}
