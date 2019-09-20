using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEngine;
using UnityEditor;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomDrawer(typeof(Dialogue), true)]
    public class DialogueDrawer : ClassDrawer
    {
        public DialogueDrawer(FieldInfo fieldInfo, object value, object parentValue, int arrayIndex)
            : base(fieldInfo, value, parentValue, arrayIndex)
        { }

        protected override int GetHeightInternal()
        {
            return children.Sum(o => o.GetHeight()) + GetExtraHeight() + ReflectionDrawerStyles.singleLineHeight * 2;
        }

        protected override object DrawInternal(Rect rect)
        {
            foreach (var child in children)
            {
                child.Draw(ref rect);
            }

            rect.y += ReflectionDrawerStyles.singleLineHeight;
            if (GUI.Button(rect, "Edit"))
            {
                DialogueEditorWindow.Edit((Dialogue)value);
            }

            return value;
        }
    }
}