using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public interface ISpawnerCallbacks
    {

//        /// <summary>
//        /// Called before an object is spawned. This gives more granular control over it's properties and values.
//        /// </summary>
//        void OnBeforeSpawn(Spawner spawner, SpawnerInfoCategory category);
        void OnSpawnedObject(SpawnerBase spawner, SpawnerCategoryInfo category, GameObject obj);

    }
}
