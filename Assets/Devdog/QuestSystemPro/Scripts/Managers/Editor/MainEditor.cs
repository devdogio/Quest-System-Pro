using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Devdog.General.Editors;
using Devdog.General.Editors.GameRules;
using EditorStyles = UnityEditor.EditorStyles;
using EditorUtility = UnityEditor.EditorUtility;


namespace Devdog.QuestSystemPro.Editors
{
    using System.Linq;

    public class MainEditor : EditorWindow
    {
        private static int toolbarIndex { get; set; }

        public static EmptyEditor itemEditor { get; set; }
        public static EmptyEditor equipEditor { get; set; }
        public static SettingsEditor settingsEditor { get; set; }

        public static List<IEditorCrud> editors = new List<IEditorCrud>(8);


        private static MainEditor _window;

        public static MainEditor window
        {
            get
            {
                if(_window == null)
                    _window = GetWindow<MainEditor>(false, "Quest System Pro Manager", false);

                return _window;
            }
        }

        protected string[] editorNames
        {
            get
            {
                string[] items = new string[editors.Count];
                for (int i = 0; i < editors.Count; i++)
                {
                    items[i] = editors[i].ToString();
                }

                return items;
            }
        }

        [MenuItem(QuestSystemPro.ToolsMenuPath + "Main editor", false, -99)] // Always at the top
        public static void ShowWindow()
        {
            _window = GetWindow<MainEditor>(false, "Quest System Pro Manager", true);
        }

        private void OnEnable()
        {
            minSize = new Vector2(600.0f, 400.0f);
            toolbarIndex = 0;

            //if (ItemManager.database == null)
            //    return;

//            _databasesInProject = AssetDatabase.FindAssets("t:" + typeof(InventoryItemDatabase).Name);

            GameRulesWindow.CheckForIssues();
            GameRulesWindow.OnIssuesUpdated += UpdateMiniToolbar;

            CreateEditors();
        }

        private void OnDisable()
        {
            GameRulesWindow.OnIssuesUpdated -= UpdateMiniToolbar;
        }

        internal static void UpdateMiniToolbar(List<IGameRule> issues)
        {
            window.Repaint();
        }

        public static void SelectTab(Type type)
        {
            int i = 0;
            foreach (var editor in editors)
            {
                var ed = editor as EmptyEditor;
                if (ed != null)
                {
                    bool isChildOf = ed.childEditors.Select(o => o.GetType()).Contains(type);
                    if (isChildOf)
                    {
                        toolbarIndex = i;
                        for (int j = 0; j < ed.childEditors.Count; j++)
                        {
                            if (ed.childEditors[j].GetType() == type)
                            {
                                ed.toolbarIndex = j;
                            }
                        }

                        toolbarIndex = i;
                        ed.Focus();
                        window.Repaint();
                        return;
                    }
                }

                if (editor.GetType() == type)
                {
                    toolbarIndex = i;
                    editor.Focus();
                    window.Repaint();
                    return;
                }

                i++;
            }

            Debug.LogWarning("Trying to select tab in main editor, but type isn't in editor.");
        }

        public virtual void CreateEditors()
        {
            editors.Clear();
            itemEditor = new EmptyEditor(QuestSystemPro.ProductName + " - Main editor", this)
            {
                requiresDatabase = true
            };

            itemEditor.childEditors.Add(new QuestsEditor("Quest", "Quests", this));
            itemEditor.childEditors.Add(new AchievementsEditor("Achievement", "Achievements", this));
            editors.Add(itemEditor);

            settingsEditor = new SettingsEditor("Settings", "Settings categories", this);
            editors.Add(settingsEditor);
        }

        protected virtual void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal();

            int before = toolbarIndex;
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, editorNames, Devdog.General.Editors.EditorStyles.toolbarStyle);
            if (before != toolbarIndex)
            {
                editors[toolbarIndex].Focus();
            }
            
            EditorGUILayout.EndHorizontal();
        }

        internal static void DrawMiniToolbar(List<IGameRule> issues)
        {
            GUILayout.BeginVertical("Toolbar", GUILayout.ExpandWidth(true));
            
            var issueCount = issues.Sum(o => o.ignore == false ? o.issues.Count : 0);
            if (issueCount > 0)
                GUI.color = Color.red;
            else
                GUI.color = Color.green;
            
            if (GUILayout.Button(issueCount + " issues found in scene.", "toolbarbutton", GUILayout.Width(300)))
            {
                GameRulesWindow.ShowWindow();
            }
            GUI.color = Color.white;

            GUILayout.EndVertical();
        }


        public void OnGUI()
        {
            DrawToolbar();

//            EditorPrefs.DeleteKey("InventorySystem_ItemPrefabPath");

            if (QuestManager.instance != null)
            {
                Devdog.General.Editors.EditorUtility.ErrorIfEmpty(EditorPrefs.GetString(SettingsEditor.PrefabSaveKey) == string.Empty, "Prefab folder is not set, items cannot be saved.");
                if (EditorPrefs.GetString(SettingsEditor.PrefabSaveKey) == string.Empty)
                {
                    GUI.enabled = true;
                    toolbarIndex = editors.Count - 1;
                    // Draw the editor
                    editors[toolbarIndex].Draw();

                    if (GUI.changed)
                    {
                        UnityEditor.EditorUtility.SetDirty(QuestManager.instance); // To make sure it gets saved.
                    }

                    GUI.enabled = false;
                    return;
                }

                if (toolbarIndex < 0 || toolbarIndex >= editors.Count || editors.Count == 0)
                {
                    toolbarIndex = 0;
                    CreateEditors();
                }

                // TODO: Error when no manager is present!

                // Draw the editor
                editors[toolbarIndex].Draw();

                if (GUI.changed)
                    UnityEditor.EditorUtility.SetDirty(QuestManager.instance); // To make sure it gets saved.
            }

            DrawMiniToolbar(GameRulesWindow.GetAllActiveRules().ToList());
        }
    }
}