using System;
using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    public abstract class SpawnerBase : MonoBehaviour
    {
        public SpawnerCategoryInfo[] categoryInfo = new SpawnerCategoryInfo[0];
        public SpawnerInfo spawnerInfo = new SpawnerInfo();
        
        /// <summary>
        /// The creator used to create and destroy objects.
        /// </summary>
        protected ISpawnerObjectCreator objectCreator;

        /// <summary>
        /// The volume used to get a spawning location for an object.
        /// </summary>
        protected ISpawnerVolume volume;

        /// <summary>
        /// Used to check if this spawner is relevant to the game. If not no spawning will take place until it becomes relevant.
        /// Not required, when left empty a null object is used, which will always be considered relevant.
        /// </summary>
        /// 
        /// <remarks>
        ///     If the player is all the way on the other side of the map this won't become relevant until the player gets closer.
        ///     When using relevancy you can delay the spawning until it's become relevant to the game.
        /// </remarks>
        protected IObjectRelevancy relevancy;

        /// <summary>
        /// Are there still enabled or disabled objects in this spawner?
        /// </summary>
        public bool hasSpawnedObjects
        {
            get
            {
                foreach (var c in categoryInfo)
                {
                    if (c.transform != null && c.transform.childCount > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        private readonly List<ISpawnerCallbacks> _callbacksCache = new List<ISpawnerCallbacks>();
        private readonly List<ISpawnerObjectCallbacks> _objectsCallbacksCache = new List<ISpawnerObjectCallbacks>();

        protected virtual void Awake()
        {
            InitCategories();
            GetImplementations();

            relevancy.OnBecameRelevant += OnBecameRelevant;
            relevancy.OnBecameIrrelevant += OnBecameIrrelevant;

            // Just in case there were existing objects left from the editor or other systems.
            DestroyAllSpawnedObjects();
        }

        protected virtual void InitCategories()
        {
            foreach (var c in categoryInfo)
            {
                var p = new GameObject(c.prefab.name + "_Parent");
                p.transform.SetParent(transform);
                p.transform.ResetTRS();

                c.transform = p.transform;
            }
        }

        protected virtual void GetImplementations()
        {
            volume = GetComponent<ISpawnerVolume>();
            if (volume == null)
            {
                volume = new RandomSpawnerVolume(20f);
            }

            objectCreator = GetComponent<ISpawnerObjectCreator>();
            if (objectCreator == null)
            {
                if (spawnerInfo.useMemoryPool)
                {
                    objectCreator = new SpawnerPooledObjectCreator();
                }
                else
                {
                    objectCreator = new SpawnerObjectCreator();
                }
            }

            relevancy = GetComponent<IObjectRelevancy>();
            if (relevancy == null)
            {
                relevancy = new ObjectNullRelevancy();
            }
        }

        /// <summary>
        /// This object became relevant and we want to spawn the objects now.
        /// </summary>
        protected virtual void OnBecameRelevant()
        {
            Assert.IsTrue(relevancy.IsRelevant(gameObject), "OnBecameRelevant was called, but the spawner still isn't considered relevant!");

            if (hasSpawnedObjects)
            {
                DevdogLogger.LogVerbose("Spawner became relevant, re-enabled old spawned objects");
            }

            SetAllCategoryObjectsActive(true);
            if (hasSpawnedObjects == false)
            {
                DevdogLogger.LogVerbose("Spawner became relevant, no previous objects, spawning now...");
                Spawn();
            }
        }

        /// <summary>
        /// This object is no longer relevant.
        /// </summary>
        protected virtual void OnBecameIrrelevant()
        {
            // Disable the object, it may become relevant again in the future.
            DevdogLogger.LogVerbose("Spawner became irrelevant, disabling previously spawned objects");
            SetAllCategoryObjectsActive(false);
        }

        public virtual void Spawn()
        {
            if (relevancy.IsRelevant(gameObject))
            {
                DoSpawn();
            }
        }

        protected virtual void DoSpawn()
        {
            // Cache all callback receivers.
            GetComponentsInChildren<ISpawnerCallbacks>(_callbacksCache);
            foreach (var cat in categoryInfo)
            {
                SpawnCategory(cat);
            }
        }

        protected virtual void SpawnCategory(SpawnerCategoryInfo category)
        {
            if (CanSpawnSingle(category) == false)
            {
                return;
            }

            // Make sure the category object is active.
            category.transform.gameObject.SetActive(true);
            for (int i = 0; i < category.amount.Generate(); i++)
            {
                if (Application.isPlaying && category.delay.max > 0f)
                {
                    StartCoroutine(WaitAndSpawnSingle(category, category.delay.Generate()));
                }
                else
                {
                    SpawnSingle(category);
                }
            }

            if (category.useIntervals && Application.isPlaying)
            {
                StartCoroutine(_DoCategoryIntervals(category, category.intervalEmitTimes));
            }
        }

        private IEnumerator _DoCategoryIntervals(SpawnerCategoryInfo category, int intervalsRemaining)
        {
            intervalsRemaining--;

            // Spawn the interval's objects
            if (CanSpawnSingle(category))
            {
                for (int i = 0; i < category.intervalSpawnAmount.Generate(); i++)
                {
                    SpawnSingle(category);
                }
            }

            yield return new WaitForSeconds(category.intervalWaitTime.Generate());
            StartCoroutine(_DoCategoryIntervals(category, intervalsRemaining));
        }

        protected IEnumerator WaitAndSpawnSingle(SpawnerCategoryInfo category, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            if (CanSpawnSingle(category))
            {
                SpawnSingle(category);
            }
        }

        protected virtual bool CanSpawnSingle(SpawnerCategoryInfo category)
        {
            return category.transform.childCount < spawnerInfo.maxObjects;
        }

        /// <summary>
        /// Spawns a single instance and returns the spawned instance.
        /// </summary>
        protected virtual GameObject SpawnSingle(SpawnerCategoryInfo category)
        {
            var inst = objectCreator.GetObject(this, category);

            inst.transform.SetParent(category.transform);
            inst.transform.localPosition = volume.GetPointInVolume(this, category) + Vector3.up * spawnerInfo.maxRaycastDistance; // + some up to avoid it getting stuck in the ground.
            inst.transform.localRotation = Quaternion.Euler(category.rotation.Generate());
            inst.transform.localScale = category.scale.Generate();

            if (category.snapToGround)
            {
                SnapObjectToGround(category, inst);
                inst.transform.Translate(category.snapToGroundOffset);
            }

            InitSpawnedObject(category, inst);
            foreach (var c in _callbacksCache)
            {
                c.OnSpawnedObject(this, category, inst);
            }

            return inst;
        }

        protected virtual void InitSpawnedObject(SpawnerCategoryInfo category, GameObject spawnedInstance)
        {
            if (spawnerInfo.enableCallbacksOnSpawnedObjects && Application.isPlaying)
            {
                spawnedInstance.GetComponents<ISpawnerObjectCallbacks>(_objectsCallbacksCache);
                foreach (var callback in _objectsCallbacksCache)
                {
                    callback.OnSpawned(this, category);
                }
            }
        }

        protected virtual void SnapObjectToGround(SpawnerCategoryInfo category, GameObject spawnedInstance)
        {
            RaycastHit hit;

            // Raycast distance is actually multiplied by 2 because when spawned the object is placed 'maxRaycastDistance' from the ground to ensure it's never stuck below it.
            if (Physics.Raycast(spawnedInstance.transform.position, -spawnedInstance.transform.up, out hit, spawnerInfo.maxRaycastDistance * 2f, spawnerInfo.layerMask, QueryTriggerInteraction.Ignore))
            {
                spawnedInstance.transform.position = hit.point;
            }
        }

        protected virtual void SetAllCategoryObjectsActive(bool active)
        {
            foreach (var c in categoryInfo)
            {
                c.transform.gameObject.SetActive(active);
            }
        }

        public virtual void DestroyAllSpawnedObjects()
        {
            foreach (var cat in categoryInfo)
            {
                DestroySpawnedObjects(cat);
            }
        }

        public virtual void DestroySpawnedObjects(SpawnerCategoryInfo category)
        {
            for (int i = category.transform.childCount - 1; i >= 0; i--)
            {
                objectCreator.DestroyObject(this, category, category.transform.GetChild(i).gameObject);
            }
        }
    }
}
