using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class Waypoint : MonoBehaviour
    {
        // Assuming single-threaded use here, all waypoints will share the cache.
        private static List<IWaypointCondition> _conditionsCache = new List<IWaypointCondition>();
        private static List<IWaypointAction> _actionsCache = new List<IWaypointAction>();

        private WaypointGroup _parent;
        public WaypointGroup parent
        {
            get
            {
                if (_parent == null)
                {
                    _parent = GetComponentInParent<WaypointGroup>();
                }

                return _parent;
            }
        }

        public virtual List<IWaypointCondition> GetWaypointConditions()
        {
            GetComponents<IWaypointCondition>(_conditionsCache);
            _conditionsCache.InsertRange(0, parent.GetGlobalWaypointConditions());
            return _conditionsCache;
        }

        public virtual List<IWaypointAction> GetWaypointActions()
        {
            GetComponents<IWaypointAction>(_actionsCache);
            _actionsCache.InsertRange(0, parent.GetGlobalWaypointActions());
            return _actionsCache;
        }
    }
}