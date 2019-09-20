using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;
using Devdog.General.Editors;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomDrawer(typeof(IfVariableEdgeCondition<>))]
    public class IfVariableEdgeConditionDrawer : ChildrenValueDrawerBase
    {
        private static DerivedTypeInformation _types;
        private int _typeIndex;

        public IfVariableEdgeConditionDrawer(FieldInfo fieldInfo, object value, object parentValue, int arrayIndex)
            : base(fieldInfo, value, parentValue, arrayIndex)
        {
            if (_types == null)
            {
                _types = ReflectionDrawerUtility.GetDerivedTypesFrom(typeof(IfVariableEdgeCondition<>), null);
            }

            _typeIndex = Mathf.Max(Array.IndexOf(_types.types, value.GetType()), 0);
        }

        protected override int GetHeightInternal()
        {
            if(FoldoutBlockUtility.IsUnFolded(this))
            {
                return base.GetHeightInternal() + ReflectionDrawerStyles.singleLineHeight;
            }

            return base.GetHeightInternal();
        }

        protected override void DrawChildren(Rect rect)
        {
            if (hideGroup)
            {
                foreach (var child in children)
                {
                    child.Draw(ref rect);
                }

                DrawVariableTypePicker(ref rect);
            }
            else
            {
                var r2 = rect;
                const int r2Padding = 15;
                r2.width = EditorGUIUtility.labelWidth - r2Padding;
                r2.x += r2Padding;

                EditorGUI.LabelField(r2, GetFoldoutName());
                using (var foldout = new FoldoutBlock(this, new Rect(rect.x, rect.y, rect.width, ReflectionDrawerStyles.singleLineHeight), GUIContent.none))
                {
                    rect.y += ReflectionDrawerStyles.singleLineHeight;

                    if (foldout.isUnFolded)
                    {
                        using (var indent = new IndentBlock(rect))
                        {
                            foreach (var child in children)
                            {
                                child.Draw(ref indent.rect);
                            }

                            DrawVariableTypePicker(ref indent.rect);
                        }
                    }
                }
            }
        }

        protected void DrawVariableTypePicker(ref Rect rect)
        {
            EditorGUI.BeginChangeCheck();
            _typeIndex = EditorGUI.Popup(rect, new GUIContent("Variable type"), _typeIndex, _types.content);
            if (EditorGUI.EndChangeCheck())
            {
                var p = (IEdgeCondition[])parentValue;
                var v = (IEdgeCondition)value;

                // Changed, replace this node editor for a new instance of the new generic type...
                var valueIndex = Array.IndexOf(p, v);
                if (valueIndex == -1)
                {
                    Debug.LogError("Couldn't find edge to change!!");
                    return;
                }

                // Do the node
                {
                    var newType = _types.types[_typeIndex];

                    value = (IEdgeCondition)Activator.CreateInstance(newType);
                    NotifyValueChanged(value);
                }

                // Update the editors
                foreach (var sidebar in DialogueEditorWindow.sidebars)
                {
                    var s = sidebar as DialogueEditorSidebarProperties;
                    if(s != null)
                    {
                        s.Update(DialogueEditorWindow.window);
                    }
                }
            }
        }
    }
}
