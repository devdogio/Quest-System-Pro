using System;
using System.Linq;
using System.Text;
using Devdog.General;
using Devdog.General.Editors.ReflectionDrawers;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public static class DialogueReflectionUtility
    {
        private static Type[] _allNodes;


        public static Type GetCustomNodeEditorFor(Type nodeType)
        {
            if (_allNodes == null)
            {
                _allNodes = ReflectionUtility.GetAllClassesWithAttribute(typeof(CustomNodeEditorAttribute)).ToArray();
            }

            foreach (var node in _allNodes)
            {
                var customEditor = (CustomNodeEditorAttribute)node.GetCustomAttributes(typeof(CustomNodeEditorAttribute), true).FirstOrDefault();
                if (customEditor != null)
                {
                    if(customEditor.type.IsGenericTypeDefinition &&
                        nodeType.IsGenericType &&
                        customEditor.type == nodeType.GetGenericTypeDefinition())
                    {
                        return node;
                    }

                    if (customEditor.type == nodeType)
                    {
                        return node;
                    }
                }
            }

            return null;
        }
    }
}
