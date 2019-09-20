using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public class SpawnerPooledObjectCreator : ISpawnerObjectCreator
    {
        private readonly Dictionary<SpawnerCategoryInfo, GameObjectPool> _pools = new Dictionary<SpawnerCategoryInfo, GameObjectPool>();

        public GameObject GetObject(SpawnerBase spawner, SpawnerCategoryInfo category)
        {
            GameObjectPool pool;
            var exists = _pools.TryGetValue(category, out pool);
            if (exists == false)
            {
                pool = new GameObjectPool(category.prefab, spawner.spawnerInfo.poolSize);
                _pools[category] = pool;
            }

            return pool.Get();
        }

        public void DestroyObject(SpawnerBase spawner, SpawnerCategoryInfo category, GameObject obj)
        {
            _pools[category].Destroy(obj);
        }
    }
}
