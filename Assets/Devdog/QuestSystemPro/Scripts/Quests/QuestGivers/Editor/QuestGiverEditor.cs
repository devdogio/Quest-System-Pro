using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEditor;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(QuestGiver), true)]
    public class QuestGiverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = (QuestGiver) target;
            var trigger = t.gameObject.GetComponent<TriggerBase>();
            if (trigger == null)
            {
                GUILayout.Space(10);

                GUILayout.Label("You can add a trigger to set the use distance.");
                foreach (var type in ReflectionUtility.GetAllTypesThatImplement(typeof(TriggerBase)))
                {
                    var tempType = type;
                    if (GUILayout.Button("Add: " + tempType.Name))
                    {
                        t.gameObject.AddComponent(tempType);
                    }
                }
            }
        }
    }
}