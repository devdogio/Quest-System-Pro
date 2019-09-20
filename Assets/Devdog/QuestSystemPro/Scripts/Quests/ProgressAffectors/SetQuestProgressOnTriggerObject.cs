using UnityEngine;
using System.Collections;
using System;
using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Set Quest Progress/Set Quest Progress On Trigger Object")]
    [RequireComponent(typeof(Trigger))]
    public sealed class SetQuestProgressOnTriggerObject : MonoBehaviour, ITriggerCallbacks
    {
        private enum Use
        {
            OnUse,
            OnUnUse
        }

        public QuestProgressDecorator progress;

        [Header("Trigger configuration")]
        [SerializeField]
        private Use _use;


        public bool OnTriggerUsed(Player player)
        {
            if (_use == Use.OnUse)
            {
                progress.Execute();
            }

            return false;
        }

        public bool OnTriggerUnUsed(Player player)
        {
            if (_use == Use.OnUnUse)
            {
                progress.Execute();
            }

            return false;
        }
    }
}