using System;
using System.Collections.Generic;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class DialogueEditorSidebarVariables : DialogueEditorSidebarBase
    {
        private Dialogue _lastDialogue;
        private DrawerBase _drawer;
        private bool _isDirty = false;

        public DialogueEditorSidebarVariables(string name)
            : base(name)
        {


        }

        public override void Draw(Rect rect, DialogueEditorWindow editor)
        {
            // Little bit of caching..
            if (_lastDialogue != editor.dialogue || _isDirty)
            {
                _lastDialogue = editor.dialogue;
                _drawer = ReflectionDrawerUtility.BuildEditorHierarchy(typeof (Dialogue).GetField("variables"), _lastDialogue);

                _isDirty = false;
            }

            _drawer.Draw(ref rect);

//            rect.y += EditorGUIUtility.singleLineHeight;
//            if (GUI.Button(rect, "Add generic type"))
//            {
//                var l = new List<Variable>(_lastDialogue.variables.Cast<Variable>());
//                l.Add(new Variable<Vector2>());
//                _lastDialogue.variables = l.ToArray();
//
//                _isDirty = true;
//            }
        }
    }
}
