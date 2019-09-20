using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEditor;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomEditor(typeof(DialogueOwner))]
    [CanEditMultipleObjects]
    public class DialogueOwnerEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = (DialogueOwner) target;

            if (t.dialogueCamera == null)
            {
                var d = t.gameObject.GetComponentInChildren<DialogueCamera>();
                if (d != null)
                {
                    if (GUILayout.Button("Set child dialogue camera"))
                    {
                        t.dialogueCamera = d;
                        EditorUtility.SetDirty(t);
                    }
                }
            }

            if (GUILayout.Button("Create new dialogue"))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save location", "Dialogue" + DateTime.Now.ToFileTime(), "asset", "Save new dialogue object location");
                if (string.IsNullOrEmpty(path) == false)
                {
                    var asset = ScriptableObject.CreateInstance<Dialogue>();
//                    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(Dialogue).ToString() + ".asset");

                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    t.dialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(path);
                    EditorUtility.SetDirty(t);

                    DialogueEditorWindow.Edit(t);
                }
            }

            if (GUILayout.Button("Edit dialogue"))
            {
                DialogueEditorWindow.Edit(t);
            }

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