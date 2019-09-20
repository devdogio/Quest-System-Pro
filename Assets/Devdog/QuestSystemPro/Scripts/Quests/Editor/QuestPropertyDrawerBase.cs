using System;
using Devdog.General.Editors;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    public class QuestPropertyDrawerBase<T> : PropertyDrawer where T : Quest
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            var r = rect;
            r.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(r, label);

            rect.width -= r.width;
            rect.x += r.width;
            if (GUI.Button(rect, new GUIContent(property.objectReferenceValue == null ? "(empty)" : property.objectReferenceValue.name), UnityEditor.EditorStyles.objectField))
            {
                var objectField = rect;
                objectField.width -= 20;

                var rightSide = rect;
                rightSide.width = 20;
                rightSide.x += objectField.width;

                if (objectField.Contains(Event.current.mousePosition) && Event.current.button == 0)
                {
                    OnClickedFieldLeftSide(property);
                }

                if (rightSide.Contains(Event.current.mousePosition) && Event.current.button == 0)
                {
                    OnClickedFieldRightSide(property);
                }
            }

            EditorGUI.EndProperty();
        }

        protected virtual void OnClickedFieldRightSide(SerializedProperty prop)
        {
            // Use custom picker only for prefabs. Use Unity's picker for scene objects and built-in types.
            ObjectPickerUtility.GetObjectPickerForType(typeof(T), (asset) =>
            {
                prop.objectReferenceValue = asset;
                prop.serializedObject.ApplyModifiedProperties();
            });
        }

        protected virtual void OnClickedFieldLeftSide(SerializedProperty prop)
        {
            EditorGUIUtility.PingObject(prop.objectReferenceValue);
        }
    }
}
