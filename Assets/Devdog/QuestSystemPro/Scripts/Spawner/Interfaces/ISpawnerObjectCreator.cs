using System;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public interface ISpawnerObjectCreator
    {

        GameObject GetObject(SpawnerBase spawner, SpawnerCategoryInfo category);
        void DestroyObject(SpawnerBase spawner, SpawnerCategoryInfo category, GameObject obj);

    }
}
