using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class SpawnerCategoryInfo
    {
        public bool enabled = true;

        [Header("Spawn settings")]
        public GameObject prefab;
        public int maxAmount = 20;
        public MinMaxRange amount = new MinMaxRange() { min = 1, max = 5 };
        public FMinMaxRange delay = new FMinMaxRange() { min = 0f, max = 0f };

        public MinMaxVector3 scale = new MinMaxVector3() { min = Vector3.one, max = Vector3.one };
        public MinMaxVector3 rotation;

        [Header("Spawn settings (Waves)")]
        public bool useIntervals = false;
        public FMinMaxRange intervalWaitTime = new FMinMaxRange() { min = 6f, max = 10f };
        public MinMaxRange intervalSpawnAmount = new MinMaxRange() { min = 2, max = 4 };
        public int intervalEmitTimes = 3; // Emit 3 times

        /// <summary>
        /// When snap to ground is enabled the object will be raycasted to the ground.
        /// </summary>
        [Header("Snapping")]
        public bool snapToGround;
        public Vector3 snapToGroundOffset;



        // Runtime info
        public Transform transform { get; set; }
    }
}
