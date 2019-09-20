#if INVENTORY_PRO

using System;
using System.Collections.Generic;
using System.Reflection;
using Devdog.General.Editors.ReflectionDrawers;
using Devdog.InventoryPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Integration.InventoryPro.Editor
{
    [CustomDrawer(typeof(ItemAmountRow))]
    public class ItemAmountRowEditor : ChildrenValueDrawerBase
    {
        public ItemAmountRowEditor(FieldInfo fieldInfo, object value, object parentValue, int arrayIndex)
            : base(fieldInfo, value, parentValue, arrayIndex)
        {
        }

        protected override int GetHeightInternal()
        {
            return ReflectionDrawerStyles.singleLineHeight;
        }

        protected override object DrawInternal(Rect rect)
        {
            Assert.AreEqual(2, children.Count);

            var r = rect;
            r.width *= 0.7f;

            var r2 = rect;
            r2.x += r.width;
            r2.width *= 0.3f;

            var before = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;

            EditorGUI.BeginChangeCheck();

            children[0].overrideFieldName = GUIContent.none;
            children[0].Draw(ref r);

            if (((uint) children[1].value) <= 0)
            {
                children[1].NotifyValueChanged(1u);
            }

            children[1].overrideFieldName = GUIContent.none;
            children[1].Draw(ref r2);

            // Constantly update, it's a struct...
            NotifyValueChanged(value);
            EditorGUIUtility.labelWidth = before;

            return value;
        }
    }
}

#endif