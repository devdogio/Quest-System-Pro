using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomDrawer(typeof(VariableRef))]
    public class VariableRefDrawer : DrawerBase
    {
        public VariableRefDrawer(FieldInfo fieldInfo, object value, object parentValue, int arrayIndex)
            : base(fieldInfo, value, parentValue, arrayIndex)
        {

        }

        protected override int GetHeightInternal()
        {
            return ReflectionDrawerStyles.singleLineHeight;
        }

        protected override object DrawInternal(Rect rect)
        {
            var r = GetSingleLineHeightRect(rect);
            var r2 = r;
            r2.width = EditorGUIUtility.labelWidth;
            r.x += r2.width;
            r.width -= r2.width;

            var variableRef = (VariableRef) value;
            var genericType = value.GetType().GetGenericArguments().FirstOrDefault();

            EditorGUI.LabelField(r2, fieldInfo.Name);
            if (DialogueEditorWindow.window != null && DialogueEditorWindow.window.dialogue)
            {
                // TODO: Think of a cleaner way then DialogueEditor... Right now variables can't be used outside of the dialogues.
                var variables = DialogueEditorWindow.window.dialogue.variables.variables.Where(o => o != null)
                            .Where(o => o.GetType().GetGenericArguments().FirstOrDefault() == genericType).ToArray();

                var names = variables.Select(o => o.name).ToArray();


                int index = 0;
                for (int i = 0; i < variables.Length; i++)
                {
                    if (variables[i].guid == variableRef.guid)
                    {
                        index = i;
                    }
                }

                EditorGUI.BeginChangeCheck();
                index = EditorGUI.Popup(r, index, names);
                if (EditorGUI.EndChangeCheck())
                {
                    variableRef.guid = variables[index].guid;
                    //Debug.Log("Change variable to " + " i: " + index + " : " + names[index]);
                }
            }

            return value;
        }

        public override DrawerBase FindDrawerRelative(string fieldName)
        {
            return null;
        }
    }
}
