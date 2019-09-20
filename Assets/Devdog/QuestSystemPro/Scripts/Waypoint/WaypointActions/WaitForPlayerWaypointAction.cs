using System;
using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public sealed class WaitForPlayerWaypointAction : MonoBehaviour, IWaypointAction
    {
        private WaitForSeconds _waitTime;
        private void Awake()
        {
            _waitTime = new WaitForSeconds(0.25f);
        }

        public IEnumerator PerformActionsAtWaypoint(Waypoint waypoint, IWaypointCharacter character)
        {
            DevdogLogger.LogVerbose("(start) Wait for player waypoint action", character.transform);
            character.characterController.Stop();

            while ((PlayerManager.instance.currentPlayer.transform.position - character.transform.position).sqrMagnitude > 9f)
            {
                yield return _waitTime;
            }

            character.characterController.Resume();
        }
    }
}
