using System;
using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    public class WaypointCharacter : MonoBehaviour, IWaypointCharacter
    {
        public WaypointGroup waypointGroup;

        private int _currentWaypointIndex;
        public int currentWaypointIndex
        {
            get { return _currentWaypointIndex; }
            protected set
            {
                if (waypointGroup.loop)
                {
                    _currentWaypointIndex = value % waypointGroup.waypoints.Length;
                }
                else
                {
                    _currentWaypointIndex = value;
                }
            }
        }

        public Waypoint currentWaypoint
        {
            get
            {
                if (currentWaypointIndex > waypointGroup.waypoints.Length - 1)
                {
                    return null;
                }

                return waypointGroup.waypoints[currentWaypointIndex];
            }
        }

        public Waypoint nextWaypoint
        {
            get
            {
                if (currentWaypointIndex + 1 > waypointGroup.waypoints.Length - 1)
                {
                    return null;
                }

                return waypointGroup.waypoints[currentWaypointIndex + 1];
            }
        }

        public IWaypointCharacterController characterController { get; set; }
        protected Coroutine customUpdateCoroutine;
        private readonly WaitForSeconds _updateNextWaypointWaitInterval = new WaitForSeconds(0.25f);


        protected void Awake()
        {
            characterController = GetComponent<IWaypointCharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<WaypointNavmeshCharacterController>();
            }
        }

        protected void OnEnable()
        {
            StartWalkingWaypointGroup();
        }

        protected void OnDisable()
        {
            StopWalking();
        }

        public void StartWalkingWaypointGroup()
        {
            StartWalkingWaypointGroup(waypointGroup);
        }

        public void StartWalkingWaypointGroup(WaypointGroup g)
        {
            this.waypointGroup = g;

            if (customUpdateCoroutine != null)
            {
                StopCoroutine(customUpdateCoroutine);
            }
            customUpdateCoroutine = StartCoroutine(UpdateNextWaypointInterval());
        }

        public void StopWalking()
        {
            if (customUpdateCoroutine != null)
            {
                StopCoroutine(customUpdateCoroutine);
            }
        }

        protected IEnumerator UpdateNextWaypointInterval()
        {
            while (true)
            {
                // Start with interval. This ensures objects are initialized before running
                yield return _updateNextWaypointWaitInterval;

                if (characterController.distanceToDestination < 1f)
                {
                    // Reached current waypoint, do actions
                    yield return StartCoroutine(PerformActionsAtWaypoint(currentWaypoint));

                    TrySelectNextWaypoint();
                    if (currentWaypoint != null)
                    {
                        characterController.SetDestination(currentWaypoint.transform.position);
                    }
                    else
                    {
                        characterController.Stop();
                        StopCoroutine(customUpdateCoroutine);
                    }
                }
            }
        }

        protected void TrySelectNextWaypoint()
        {
            var conditions = currentWaypoint.GetWaypointConditions();
            if (conditions.TrueForAll(o => o.CanMoveToNextWaypoint(this)))
            {
                currentWaypointIndex++;
            }
        }

        public void UseWaypointGroup(WaypointGroup group, bool startAtClosestWaypoint)
        {
            if (startAtClosestWaypoint)
            {
                var info = group.FindClosestWaypoint(transform.position);
                if (info.waypoint != null)
                {
                    UseWaypointGroup(group, info.index);
                }
                else
                {
                    DevdogLogger.LogWarning("Couldn't find closest waypoint in waypointGroup", this);
                }
            }
            else
            {
                UseWaypointGroup(group, 0);
            }
        }

        public void UseWaypointGroup(WaypointGroup group, int startWaypointIndex)
        {
            Assert.IsNotNull(group);

            this.waypointGroup = group;
            currentWaypointIndex = startWaypointIndex;

            DevdogLogger.LogVerbose("Waypoint character controller is following new WaypointGroup", this);
        }

        public IEnumerator PerformActionsAtWaypoint(Waypoint waypoint)
        {
            var actions = waypoint.GetWaypointActions();
            for (int i = 0; i < actions.Count; i++)
            {
                yield return StartCoroutine(actions[i].PerformActionsAtWaypoint(waypoint, this));
            }
        }
    }
}
