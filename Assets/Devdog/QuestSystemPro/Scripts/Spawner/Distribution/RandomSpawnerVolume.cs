using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public sealed class RandomSpawnerVolume : MonoBehaviour, ISpawnerVolume
    {
        private readonly float _radius;
        public RandomSpawnerVolume(float radius)
        {
            _radius = radius;
        }

        public Vector3 GetPointInVolume(SpawnerBase spawner, SpawnerCategoryInfo category)
        {
            return UnityEngine.Random.insideUnitSphere * _radius + Vector3.up * (_radius / 2f); // + offset to spawn above the lower half of the sphere
        }
    }
}
