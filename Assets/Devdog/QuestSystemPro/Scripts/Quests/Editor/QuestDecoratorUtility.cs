using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    public static class QuestDecoratorUtility
    {
        private const string QuestTaskPickerDropdownKey = "Devdog_QuestTaskPickerDropdownKey";
        private static bool _questTaskPickerVal = true;

        static QuestDecoratorUtility()
        {
            _questTaskPickerVal = EditorPrefs.GetBool(QuestTaskPickerDropdownKey, true);
        }

        public static string DrawQuestTaskField(Rect position, Quest quest, string taskName)
        {
            position.width -= 25f;
            var togglePos = position;
            togglePos.width = 20f;
            togglePos.x += position.width + 5;

            if (_questTaskPickerVal)
            {
                // Use a dropdown instead of a input field.
                var labelPos = position;
                labelPos.width = EditorGUIUtility.labelWidth;
                
                EditorGUI.PrefixLabel(labelPos, new GUIContent("Task Name"));

                position.width -= labelPos.width;
                position.x += labelPos.width;

                if (quest != null)
                {
                    var index = EditorGUI.Popup(position, IndexOf(quest.tasks, taskName), quest.tasks.Select(o => o.key).ToArray());
                    if (index >= 0)
                    {
                        taskName = quest.tasks[index].key;
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, "No quest");
                }
            }
            else
            {
                taskName = EditorGUI.TextField(position, new GUIContent("Task Name"), taskName);
            }

            EditorGUI.BeginChangeCheck();
            _questTaskPickerVal = EditorGUI.Toggle(togglePos, GUIContent.none, _questTaskPickerVal);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(QuestTaskPickerDropdownKey, _questTaskPickerVal);
            }

            return taskName;
        }

        private static int IndexOf(Task[] tasks, string key)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].key == key)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
