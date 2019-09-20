using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomDrawer(typeof(Quest), true)]
    public class QuestDrawer : ClassDrawer
    {
        private string _lastControlName = "";

        public QuestDrawer(FieldInfo fieldInfo, object value, object parentValue, int arrayIndex)
            : base(fieldInfo, value, parentValue, arrayIndex)
        {
        }

        protected override int GetHeightInternal()
        {
            return children.Sum(o => o.GetHeight()); // + ReflectionDrawerStyles.singleLineHeight
        }

        protected override object DrawInternal(Rect rect)
        {
            var quest = (Quest)value;
            foreach (var child in children)
            {
                EditorGUI.BeginChangeCheck();
                child.Draw(ref rect);
                bool changed = EditorGUI.EndChangeCheck();

                if (child.fieldName.text == "name" && changed)
                {
                    _lastControlName = "name";
                }
            }


            if (_lastControlName == "name" && GUI.GetNameOfFocusedControl() != "name")
            {
                _lastControlName = "";

                var str = quest.name.message;
                str = str.Replace(' ', '_');
                str += "_";
                str += "#" + quest.ID;
                str += "_" + DateTime.Now.ToFileTime();

                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(quest), str);
//                DevdogLogger.LogVerbose("Changed name to " + str);
            }


            return value;
        }
    }
}
