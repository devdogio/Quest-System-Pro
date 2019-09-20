using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Devdog.General;
using Devdog.General.Editors;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    public class QuestsEditor : EditorCrudBase<Quest>
    {
        protected class TypeFilter
        {
            public System.Type type { get; set; }
            public bool enabled { get; set; }

            public TypeFilter(System.Type type, bool enabled)
            {
                this.type = type;
                this.enabled = enabled;
            }
        }


        private Quest _selectedItem;
        protected override Quest selectedItem
        {
            get { return _selectedItem; }
            set
            {
                _editor = null;
                if (value != null)
                {
                    _editor = Editor.CreateEditor(value);
                }

                _selectedItem = value;
            }
        }

        protected override List<Quest> crudList
        {
            get { return new List<Quest>(QuestManager.instance.quests); }
            set
            {
                QuestManager.instance.quests = value.ToArray();
                UnityEditor.EditorUtility.SetDirty(QuestManager.instance.questDatabase);
            }
        }

        private string _previouslySelectedGUIItemName;
        private static Quest _previousItem;

        private List<TypeFilter> _allItemTypes;
        private UnityEditor.Editor _editor;

        protected List<TypeFilter> allItemTypes
        {
            get
            {
                if (_allItemTypes == null)
                    _allItemTypes = GetAllItemTypes();

                return _allItemTypes;
            }
            set { _allItemTypes = value; }
        }


        //private Editor previewEditor;


        public QuestsEditor(string singleName, string pluralName, EditorWindow window)
            : base(singleName, pluralName, window)
        {
            window.autoRepaintOnSceneChange = false;
        }

        protected virtual List<TypeFilter> GetAllItemTypes()
        {
            return
                ReflectionUtility.GetAllTypesThatImplement(typeof (Quest))
                    .Select(o => new TypeFilter(o, false))
                    .ToList();
        }

        protected override bool MatchesSearch(Quest achievement, string searchQuery)
        {
            searchQuery = searchQuery.ToLower();
            return achievement.name.message.ToLower().Contains(searchQuery) ||
                achievement.description.message.ToLower().Contains(searchQuery) ||
                achievement.GetType().Name.ToLower().Contains(searchQuery);
        }

        protected override void CreateNewItem()
        {
            var picker = ScriptPickerEditor.Get(typeof(Quest), typeof(Achievement));
            picker.OnPickObject = (type) =>
            {
                var asset = (Quest)ScriptableObjectUtility.CreateAsset(type, EditorPrefs.GetString(SettingsEditor.PrefabSaveKey), GetAssetName());
                
                AddItem(asset, true);
                picker.Close();
            };

            picker.Show();
        }

        public override void DuplicateItem(int index)
        {
            var source = crudList[index];
            var newPath = AssetDatabase.GetAssetPath(source).Replace(".asset", "") + "_duplicate.asset";

            bool copied = AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(source), newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if (copied)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Quest>(newPath);
                UnityEditor.EditorUtility.SetDirty(asset); // To save it.
                UnityEditor.EditorUtility.SetDirty(QuestManager.instance); // To save it.

                AddItem(asset);
            }

            window.Repaint();
        }

        public override void AddItem(Quest item, bool editOnceAdded = true)
        {
            base.AddItem(item, editOnceAdded);

            UpdateAssetID(item);
            UpdateAssetName(item);

            UnityEditor.EditorUtility.SetDirty(QuestManager.instance);
        }

        private void UpdateAssetID(Quest item)
        {
            if(crudList == null || crudList.Count == 0)
            {
                item.ID = 0;
            }
            else
            {
                item.ID = crudList.Max(o => o.ID) + 1;
            }
            
            UnityEditor.EditorUtility.SetDirty(item);
        }

        public override void RemoveItem(int i)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(QuestManager.instance.quests[i]));
            base.RemoveItem(i);
        }

        public override void EditItem(Quest quest, int index)
        {
            base.EditItem(quest, index);

            Undo.ClearUndo(_previousItem);
            Undo.RecordObject(quest, QuestSystemPro.ProductName + "_item");

            _previousItem = quest;
        }

        protected override void DrawSidebar()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            int i = 0;
            foreach (var type in allItemTypes)
            {
                if(i % 3 == 0)
                    GUILayout.BeginHorizontal();

                type.enabled = GUILayout.Toggle(type.enabled, type.type.Name, "OL Toggle");
                
                if (i % 3 == 2 || i == allItemTypes.Count - 1)
                    GUILayout.EndHorizontal();

                i++;
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            base.DrawSidebar();

        }

        protected override void DrawSidebarRow(Quest item, int i)
        {
            int checkedCount = 0;
            foreach (var type in allItemTypes)
            {
                if (type.enabled)
                    checkedCount++;
            }

            if (checkedCount > 0)
            {
                if (allItemTypes.FirstOrDefault(o => o.type == item.GetType() && o.enabled) == null)
                {
                    return;
                }
            }

            BeginSidebarRow(item, i);

            DrawSidebarRowElement(item.name.message, 160);
            DrawSidebarRowElement(item.GetType().Name, 120);
            bool t = DrawSidebarRowElementToggle(true, "", "AssetLabel Icon", 20);
            if (t == false) // User clicked view icon
                AssetDatabase.OpenAsset(selectedItem);

            EndSidebarRow(item, i);
        }

        protected override void DrawDetail(Quest item, int index)
        {
            EditorGUIUtility.labelWidth = Devdog.General.Editors.EditorStyles.labelWidth;

            Devdog.General.Editors.EditorUtility.ErrorIfEmpty(EditorPrefs.GetString(SettingsEditor.PrefabSaveKey) == string.Empty, QuestSystemPro.ProductName + " prefab folder is not set, items cannot be saved! Please go to settings and define the " + QuestSystemPro.ProductName + " prefab folder.");
            if (EditorPrefs.GetString(SettingsEditor.PrefabSaveKey) == string.Empty)
            {
                canCreateItems = false;
                return;
            }
            canCreateItems = true;
            
            if (_editor != null)
            {
                _editor.OnInspectorGUI();
            }

            if (_previouslySelectedGUIItemName == "Quest_Name" && GUI.GetNameOfFocusedControl() != _previouslySelectedGUIItemName)
            {
                UpdateAssetName(item);
            }
            
            _previouslySelectedGUIItemName = GUI.GetNameOfFocusedControl();
            EditorGUIUtility.labelWidth = 0;
        }

        private static string GetAssetName()
        {
            return DateTime.Now.ToFileTime() + "_PFB";
        }

        protected static void UpdateAssetName(Quest item)
        {
            string name = item.GetType().Name;
            name += item.name.message.Replace(' ', '_');
            name += ".asset";

            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(item), name);
        }

        //        public void ConvertThisToNewType(Affector affector, Type type)
        //        {
        //            selectedItem = null;
        //
        //            var asset = (Affector)ScriptableObject.CreateInstance(type);
        //            var newPath = AssetDatabase.GetAssetPath(affector);
        //            AssetDatabase.CreateAsset(asset, newPath);
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        //
        //            var toFields = new List<FieldInfo>(64);
        //            EditorUtility.GetAllFieldsInherited(asset.GetType(), toFields);
        //
        //            var currentFields = new List<FieldInfo>(64);
        //            EditorUtility.GetAllFieldsInherited(affector.GetType(), currentFields);
        //            foreach (var fieldInfo in currentFields)
        //            {
        //                var toField = toFields.FirstOrDefault(o => o.Name == fieldInfo.Name);
        //                if (toField != null)
        //                {
        //                    toField.SetValue(asset, fieldInfo.GetValue(affector));
        //                }
        //            }
        //
        //            for (int i = 0; i < AffectorProManager.instance.affectors.Length; i++)
        //            {
        //                if (AffectorProManager.instance.affectors[i] == null)
        //                {
        //                    AffectorProManager.instance.affectors[i] = asset;
        //                    break;
        //                }
        //            }
        //
        //            selectedItem = asset;
        //            UnityEditor.EditorUtility.SetDirty(asset);
        //            UnityEditor.EditorUtility.SetDirty(AffectorProManager.instance);
        //
        //            GUI.changed = true;
        //            window.Repaint();
        //        }

        protected override bool IDsOutOfSync()
        {
            return crudList.Contains(null);
        }

        protected override void SyncIDs()
        {
            var l = new List<Quest>(crudList);
            l.RemoveAll(o => o == null);
            crudList = l;

            UnityEditor.EditorUtility.SetDirty(QuestManager.instance);
        }
    }
}
