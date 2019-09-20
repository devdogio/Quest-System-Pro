using System;
using UnityEngine;
using System.Collections;

namespace Devdog.QuestSystemPro
{
    public interface IWaypointCharacter
    {
        Transform transform { get; }
        IWaypointCharacterController characterController { get; set; }

        IEnumerator PerformActionsAtWaypoint(Waypoint waypoint);
    }
}