using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomPropertyDrawer(typeof(QuestProgressDecorator))]
    public class QuestProgressDecoratorEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 6f + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var quest = property.FindPropertyRelative("quest");
            var questObj = (Quest)quest.objectReferenceValue;

            var taskName = property.FindPropertyRelative("taskName");
            var type = property.FindPropertyRelative("type");
            var progress = property.FindPropertyRelative("progress");
            var useTaskProgressCap = property.FindPropertyRelative("useTaskProgressCap");

            EditorGUI.LabelField(position, new GUIContent("Quest progress"), UnityEditor.EditorStyles.boldLabel);
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(position, quest, new GUIContent(quest.displayName));
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            if (string.IsNullOrEmpty(taskName.stringValue) || (questObj != null && questObj.GetTask(taskName.stringValue) == null))
            {
                GUI.color = Color.yellow;
            }

            taskName.stringValue = QuestDecoratorUtility.DrawQuestTaskField(position, questObj, taskName.stringValue);
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            GUI.color = Color.white;

            EditorGUI.PropertyField(position, type, new GUIContent(type.displayName));
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(position, progress, new GUIContent(progress.displayName));
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(position, useTaskProgressCap, new GUIContent(useTaskProgressCap.displayName));
        }
    }
}