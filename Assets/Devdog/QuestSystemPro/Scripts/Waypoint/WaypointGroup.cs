using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public sealed class WaypointGroup : MonoBehaviour
    {
        public Waypoint[] waypoints = new Waypoint[0];
        public bool loop = false;


        // Assuming single-threaded use here, all waypoints will share the cache.
        private static List<IWaypointCondition> _conditionsCache = new List<IWaypointCondition>();
        private static List<IWaypointAction> _actionsCache = new List<IWaypointAction>();

        /// <summary>
        /// Center the pivot of this group based on all waypoints, mostly useful for editor tidyness.
        /// </summary>
        public void CenterPivot()
        {
            Vector3 newCenter = Vector3.zero;
            foreach (var waypoint in waypoints)
            {
                newCenter += waypoint.transform.localPosition;
            }

            newCenter /= waypoints.Length;
            foreach (var waypoint in waypoints)
            {
                waypoint.transform.localPosition -= newCenter;
            }

            transform.position += newCenter;
        }

        private Waypoint CreateWaypoint(Vector3 localWaypointPosition)
        {
            var obj = new GameObject("Waypoint");
            obj.transform.SetParent(transform);
            obj.transform.ResetTRS();
            obj.transform.localPosition = localWaypointPosition;

            return obj.AddComponent<Waypoint>();
        }

        public void AddWaypoint(Vector3 position)
        {
            var l = waypoints.ToList();
            l.Add(CreateWaypoint(position));
            waypoints = l.ToArray();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void InsertWaypointAfter(Vector3 position, int index)
        {
            index = Mathf.Clamp(index, 0, waypoints.Length);

            var l = waypoints.ToList();
            l.Insert(index, CreateWaypoint(position));
            waypoints = l.ToArray();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void RemoveWaypoint(Waypoint waypoint)
        {
            var l = waypoints.ToList();
            l.Remove(waypoint);
            waypoints = l.ToArray();

#if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(waypoint.gameObject);
            UnityEditor.EditorUtility.SetDirty(this);
#else
            Destroy(waypoint.gameObject);
#endif
        }

        public void RemoveWaypoint(int index)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(waypoints[index].gameObject);
#else
            Destroy(waypoints[index].gameObject);
#endif
            var l = waypoints.ToList();
            l.RemoveAt(index);
            waypoints = l.ToArray();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Get the conditions that count for all the child waypoints.
        /// </summary>
        public List<IWaypointCondition> GetGlobalWaypointConditions()
        {
            GetComponents<IWaypointCondition>(_conditionsCache);
            return _conditionsCache;
        }

        /// <summary>
        /// Get the actions that count for all the child waypoints.
        /// </summary>
        public List<IWaypointAction> GetGlobalWaypointActions()
        {
            GetComponents<IWaypointAction>(_actionsCache);
            return _actionsCache;
        }

        public WaypointInfo FindClosestWaypoint(Vector3 worldPosition)
        {
            var info = new WaypointInfo();

            float? closestSqrMagnitude = null;
            int? waypointIndex = null;
            Waypoint closestWaypoint = null;
            for (int i = 0; i < waypoints.Length; i++)
            {
                var waypoint = waypoints[i];

                var dir = waypoint.transform.position - worldPosition;
                var distSqr = dir.sqrMagnitude;
                if (closestSqrMagnitude == null || distSqr < closestSqrMagnitude)
                {
                    closestSqrMagnitude = distSqr;
                    closestWaypoint = waypoint;
                    waypointIndex = i;
                }
            }

            info.waypoint = closestWaypoint;
            info.owner = this;
            info.index = waypointIndex ?? 0;

            return info;
        }
    }
}