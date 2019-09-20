using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomPropertyDrawer(typeof(QuestStatusDecorator))]
    public class QuestStatusDecoratorEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var useTaskStatus = property.FindPropertyRelative("useTaskStatus");
            if (useTaskStatus.boolValue)
            {
                return EditorGUIUtility.singleLineHeight * 5f + 8f;
            }

            return EditorGUIUtility.singleLineHeight * 4f + 6f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var quest = property.FindPropertyRelative("quest");
            var questObj = (Quest)quest.objectReferenceValue;
            var questStatus = property.FindPropertyRelative("questStatus");

            var useTaskStatus = property.FindPropertyRelative("useTaskStatus");
            var taskName = property.FindPropertyRelative("taskName");
            var taskStatus = property.FindPropertyRelative("taskStatus");

            EditorGUI.LabelField(position, new GUIContent("Quest status"), UnityEditor.EditorStyles.boldLabel);
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(position, quest, new GUIContent("Quest"));
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            if (useTaskStatus.boolValue == false)
            { 
                EditorGUI.PropertyField(position, questStatus, new GUIContent("Quest Status"));
                position.y += EditorGUIUtility.singleLineHeight + 2f;
            }

            EditorGUI.PropertyField(position, useTaskStatus, new GUIContent("Use Task Status"));
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            if (useTaskStatus.boolValue)
            {
                if (string.IsNullOrEmpty(taskName.stringValue) || (questObj != null && questObj.GetTask(taskName.stringValue) == null))
                {
                    GUI.color = Color.yellow;
                }

                taskName.stringValue = QuestDecoratorUtility.DrawQuestTaskField(position, questObj, taskName.stringValue);
                position.y += EditorGUIUtility.singleLineHeight + 2f;

                GUI.color = Color.white;

                EditorGUI.PropertyField(position, taskStatus, new GUIContent("Task Status"));
            }
        }
    }
}