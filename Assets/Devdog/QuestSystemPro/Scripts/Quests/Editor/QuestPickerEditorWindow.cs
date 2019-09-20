using System;
using System.Collections.Generic;
using System.Linq;
using Devdog.General.Editors;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomObjectPicker(typeof(Quest), 10)]
    public class QuestPickerEditorWindow : ObjectPickerBaseEditor
    {
        public override void Init()
        {
            base.Init();

            foundObjects = foundObjects.OrderBy(o => o.GetType().Name).ThenBy(o => ((Quest)o).ID).ToList();
        }

        protected override IEnumerable<Object> FindAssetsOfType(Type type, bool allowInherited)
        {
            if (QuestManager.instance == null)
            {
                return new Object[0];
            }

            return QuestManager.instance.quests;
        }

        protected override IEnumerable<Object> FindAssetsWithComponent(Type type, bool allowInherited)
        {
            return new Object[0];
        }

        public override bool IsSearchMatch(Object asset, string searchQuery)
        {
            searchQuery = searchQuery.ToLower();
            var q = asset as Quest;
            if (q != null)
            {
                return q.name.message.ToLower().Contains(searchQuery) ||
                       q.description.message.ToLower().Contains(searchQuery) ||
                       q.ID.ToString().Contains(searchQuery);
//                       q.tasks.Any(o => o.key.ToLower().Contains(searchQuery) || o.description.ToLower().Contains(searchQuery))
            }

            return base.IsSearchMatch(asset, searchQuery);
        }

        protected override void DrawObject(Rect r, Object asset)
        {
            using (new GroupBlock(r, GUIContent.none, "box"))
            {
                var q = asset as Quest;
                if (q != null)
                {
                    var cellSize = r.width;

                    var labelRect = new Rect(5, 5, cellSize, EditorGUIUtility.singleLineHeight);
                    GUI.Label(labelRect, asset.GetType().Name, UnityEditor.EditorStyles.boldLabel);

                    labelRect.y += EditorGUIUtility.singleLineHeight;
                    GUI.Label(labelRect, q.name.message);

                    labelRect.y += EditorGUIUtility.singleLineHeight;
                    GUI.Label(labelRect, q.tasks.Length + " tasks");

//                    var iconSize = Mathf.RoundToInt(cellSize * 0.6f);
//                    GUI.DrawTexture(new Rect(cellSize * 0.2f, cellSize * 0.4f - InnerPadding, iconSize, iconSize), AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(asset)));

                    return;
                }
            }

            base.DrawObject(r, asset);
        }
    }
}
