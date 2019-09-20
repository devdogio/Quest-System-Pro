using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class SpawnerInfo
    {
        [Header("Config")]
        public bool enableCallbacksOnSpawnedObjects = true;
        public int maxObjects = 20;

        [Header("Raycasting")]
        public LayerMask layerMask = -1;
        public float maxRaycastDistance = 100f;

        /// <summary>
        /// A pool can prevent hickups when many objects are being spawned and destroyed.
        /// When you only spawn objects once and they won't be destroyed for a while leave unchecked.
        /// 
        /// <remarks>
        ///     Uses more memory in return for faster spawning / despawning.
        /// </remarks>
        /// 
        /// </summary>
        [Header("Pooling")]
        public bool useMemoryPool = false;
        public int poolSize = 32;
    }
}
