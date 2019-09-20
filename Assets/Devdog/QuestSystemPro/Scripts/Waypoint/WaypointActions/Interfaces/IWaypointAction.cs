using System;
using UnityEngine;
using System.Collections;

namespace Devdog.QuestSystemPro
{
    public interface IWaypointAction
    {

        IEnumerator PerformActionsAtWaypoint(Waypoint waypoint, IWaypointCharacter character);

    }
}