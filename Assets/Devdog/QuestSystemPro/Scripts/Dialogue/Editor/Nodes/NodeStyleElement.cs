using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public struct NodeStyleElement
    {
        public GUIStyle normal;
        public GUIStyle active;
        public Color contentColor;

        public NodeStyleElement(GUIStyle normal, GUIStyle active, Color contentColor)
        {
            this.normal = normal;
            this.active = active;
            this.contentColor = contentColor;
        }
    }
}
