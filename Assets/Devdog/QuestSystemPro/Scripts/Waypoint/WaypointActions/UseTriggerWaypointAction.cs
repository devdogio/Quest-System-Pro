using System;
using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public sealed class UseTriggerWaypointAction : MonoBehaviour, IWaypointAction
    {
        [Required]
        public Trigger trigger;

        public float useDistance = 1f;

        public IEnumerator PerformActionsAtWaypoint(Waypoint waypoint, IWaypointCharacter character)
        {
            DevdogLogger.LogVerbose("(start) Use trigger waypoint action", character.transform);
            character.characterController.SetDestination(trigger.transform.position);

            // Wait to reach trigger.
            while (character.characterController.distanceToDestination > useDistance)
            {
                yield return null;
            }

            DevdogLogger.LogVerbose("(complete) Force use trigger", character.transform);
            trigger.ForceUseWithoutStateAndUI(character.transform.gameObject);
        }
    }
}
