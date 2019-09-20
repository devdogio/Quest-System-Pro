using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public interface IWaypointCharacterController
    {
        float distanceToDestination { get; }


        /// <summary>
        /// Set the pathfinding destination for this character.
        /// </summary>
        /// <param name="worldPosition"></param>
        void SetDestination(Vector3 worldPosition);

        /// <summary>
        /// Stop the object we're controlling.
        /// </summary>
        void Stop();

        void Resume();

    }
}
