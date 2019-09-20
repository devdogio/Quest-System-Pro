using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public abstract class DialogueEditorSidebarBase
    {
        public string name { get; protected set; }



        protected DialogueEditorSidebarBase(string name)
        {
            this.name = name;
        }



        public abstract void Draw(Rect rect, DialogueEditorWindow editor);


    }
}
