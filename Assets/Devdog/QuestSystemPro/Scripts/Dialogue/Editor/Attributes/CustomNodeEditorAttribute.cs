using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomNodeEditorAttribute : Attribute
    {
        public Type type { get; set; }

        public CustomNodeEditorAttribute(Type type)
        {
            this.type = type;
        }
    }
}
