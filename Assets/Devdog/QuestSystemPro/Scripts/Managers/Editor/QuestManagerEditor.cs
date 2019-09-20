using System;
using UnityEngine;
using System.Collections;
using Devdog.General.Editors;
using UnityEditor;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(QuestManager), true)]
    public class QuestManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            if (GUILayout.Button("Generate and link databases"))
            {
                var t = (QuestManager) target;

                var path = UnityEditor.EditorUtility.SaveFolderPanel("Save to folder", "", "Save");
                if (string.IsNullOrEmpty(path) == false)
                {
                    path = "Assets" + path.Replace(Application.dataPath, ""); // To relative path

                    var settings = (SettingsDatabase)ScriptableObjectUtility.CreateAsset(typeof(SettingsDatabase), path, "Settings_" + DateTime.Now.ToFileTime() + ".asset");
                    var quests = (QuestDatabase)ScriptableObjectUtility.CreateAsset(typeof(QuestDatabase), path, "Quests_" + DateTime.Now.ToFileTime() + ".asset");
                    var language = (LanguageDatabase)ScriptableObjectUtility.CreateAsset(typeof(LanguageDatabase), path, "Language_" + DateTime.Now.ToFileTime() + ".asset");
                    
                    t.settingsDatabase = settings;
                    t.questDatabase = quests;
                    t.languageDatabase = language;

                    UnityEditor.EditorUtility.SetDirty(t);
                }
            }
        }
    }
}