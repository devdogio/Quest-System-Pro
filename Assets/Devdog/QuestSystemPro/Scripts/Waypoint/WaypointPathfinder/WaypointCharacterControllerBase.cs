using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public abstract class WaypointCharacterControllerBase : MonoBehaviour, IWaypointCharacterController
    {
        public abstract float distanceToDestination { get; }

        protected virtual void Awake()
        {
            
        }

        public abstract void SetDestination(Vector3 worldPosition);
        public abstract void Stop();
        public abstract void Resume();
    }
}
