using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// A callback that is called on the spawnwed object. This is useful when you want to initialize an object when it's spawned.
    /// Normally you can use Start() or Awake() or OnEnable() however, because objects can be pooled Start and Awake won't be called everytime.
    /// 
    /// Note: Pooled objects also receive the IPoolable callback to reset their state.
    /// </summary>
    public interface ISpawnerObjectCallbacks
    {

        void OnSpawned(SpawnerBase spawner, SpawnerCategoryInfo category);

    }
}
