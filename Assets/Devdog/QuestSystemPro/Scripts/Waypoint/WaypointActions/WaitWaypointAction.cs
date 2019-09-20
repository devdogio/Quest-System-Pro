using System;
using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public sealed class WaitWaypointAction : MonoBehaviour, IWaypointAction
    {
        public float waitTime = 1f;

        private WaitForSeconds _waitTime;
        private void Awake()
        {
            _waitTime = new WaitForSeconds(waitTime);
        }

        public IEnumerator PerformActionsAtWaypoint(Waypoint waypoint, IWaypointCharacter character)
        {
            DevdogLogger.LogVerbose("(start) Wait at waypoint action", character.transform);
            character.characterController.Stop();
            yield return _waitTime;
            character.characterController.Resume();
        }
    }
}
