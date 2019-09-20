using System;
using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public sealed class PlayAudioClipWaypointAction : MonoBehaviour, IWaypointAction
    {
        public AudioClipInfo audioClip;
        public bool playOnWaypointCharacter = true;

        public IEnumerator PerformActionsAtWaypoint(Waypoint waypoint, IWaypointCharacter character)
        {
            AudioSource source = GetComponent<AudioSource>();
            if (playOnWaypointCharacter)
            {
                source = character.transform.GetComponent<AudioSource>();
            }

            if (source != null)
            {
                source.Play(audioClip);
            }

            yield break;
        }
    }
}
