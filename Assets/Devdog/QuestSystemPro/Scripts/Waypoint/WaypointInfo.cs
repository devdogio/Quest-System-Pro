using UnityEngine;
using System.Collections;

namespace Devdog.QuestSystemPro
{
    public sealed class WaypointInfo
    {
        public Waypoint waypoint { get; set; }

        public WaypointGroup owner { get; set; }
        public int index { get; set; }
    }
}