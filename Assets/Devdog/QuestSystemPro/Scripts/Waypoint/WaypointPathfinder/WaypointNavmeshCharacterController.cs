using System;
using Devdog.General;
using UnityEngine;

#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace Devdog.QuestSystemPro
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class WaypointNavmeshCharacterController : WaypointCharacterControllerBase
    {
        public override float distanceToDestination
        {
            get { return agent.remainingDistance; }
        }

        private NavMeshAgent _agent;
        public NavMeshAgent agent
        {
            get
            {
                if (_agent == null)
                {
                    _agent = GetComponent<NavMeshAgent>();
                    _agent.autoBraking = false;
                }

                return _agent;
            }
        }

        public override void SetDestination(Vector3 worldPosition)
        {
            agent.SetDestination(worldPosition);
        }

        public override void Stop()
        {
#if UNITY_2017_1_OR_NEWER
            agent.isStopped = true;
#else
            agent.Stop();
#endif
        }

        public override void Resume()
        {
#if UNITY_2017_1_OR_NEWER
            agent.isStopped = false;
#else
            agent.Resume();
#endif
        }
    }
}
