using UnityEngine;
using System.Collections;
using System.Reflection;
using Devdog.General;
using Devdog.General.Editors;
using UnityEditor;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(SpawnerBase), true)]
    public class SpawnerBaseEditor : Editor
    {

        private ModuleList<IObjectRelevancy> _relevancyList;
        private ModuleList<ISpawnerVolume> _spawnerVolume;

        protected void OnEnable()
        {
            var t = (SpawnerBase)target;
            foreach (Transform child in t.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            _relevancyList = new ModuleList<IObjectRelevancy>(t, this, "Object relevacny");
            _relevancyList.description = "Object relevancy can be used to detect if the spawner should spawn it's objects or destroy them.";
            _relevancyList.allowMultipleImplementations = false;

            _spawnerVolume = new ModuleList<ISpawnerVolume>(t, this, "Spawner volume");
            _spawnerVolume.description = "Spawner volumes can be used to define distribution of spawned objects.";
            _spawnerVolume.allowMultipleImplementations = false;

            TryInit(t);
            TryInit(t.GetComponent<IObjectRelevancy>() as UnityEngine.Object);
            TryInit(t.GetComponent<ISpawnerVolume>() as UnityEngine.Object);
        }

        private void TryInit(UnityEngine.Object t)
        {
            if (t != null)
            {
                var awake = t.GetType().GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (awake != null)
                {
                    awake.Invoke(t, null);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _relevancyList.DoLayoutList();
            _spawnerVolume.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor tools", UnityEditor.EditorStyles.boldLabel);
            var t = (SpawnerBase) target;
            if (t.hasSpawnedObjects)
            {
                if (GUILayout.Button("Delete preview items"))
                {
                    t.DestroyAllSpawnedObjects();
                }
            }
            else
            {
                if (GUILayout.Button("Test spawn"))
                {
                    TryInit(t.GetComponent<IObjectRelevancy>() as UnityEngine.Object);
                    TryInit(t.GetComponent<ISpawnerVolume>() as UnityEngine.Object);

                    t.Spawn();
                }
            }
        }
    }
}