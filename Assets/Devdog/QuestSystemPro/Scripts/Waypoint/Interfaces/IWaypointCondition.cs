using System;
using UnityEngine;
using System.Collections;

namespace Devdog.QuestSystemPro
{
    public interface IWaypointCondition
    {

        bool CanMoveToNextWaypoint(IWaypointCharacter character);

    }
}