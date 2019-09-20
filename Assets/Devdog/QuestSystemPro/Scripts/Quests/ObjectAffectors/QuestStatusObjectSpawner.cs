using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Animates an object when the quest status is set.
    /// </summary>
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Quest Object Affectors/Object Spawner")]
    public sealed class QuestStatusObjectSpawner : QuestStatusObjectBase
    {
        [Header("Object")]
        public UnityEngine.GameObject prefab;

        [Tooltip("Create to the correct or incorrect state on game start?")]
        public bool syncStateOnStart = true;


        private UnityEngine.Object _instance;
        protected override void Awake()
        {
            base.Awake();
            questStatus.syncStateOnCallbackRegistration = syncStateOnStart;
        }

        protected override void OnStatusChangedCorrect(Quest self)
        {
            _instance = Instantiate(prefab, transform.position, transform.rotation);
        }

        protected override void OnStatusChangedInCorrect(Quest self)
        {
            if (_instance != null)
            {
                Destroy(_instance);
                _instance = null;
            }
        }
    }
}
