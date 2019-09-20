using System;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public class SpawnerObjectCreator : ISpawnerObjectCreator
    {
        public GameObject GetObject(SpawnerBase spawner, SpawnerCategoryInfo category)
        {
            return UnityEngine.Object.Instantiate<GameObject>(category.prefab);
        }

        public void DestroyObject(SpawnerBase spawner, SpawnerCategoryInfo category, GameObject obj)
        {
#if UNITY_EDITOR

            if (Application.isPlaying == false)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }

#else
            UnityEngine.Object.Destroy(obj);
#endif
        }
    }
}
