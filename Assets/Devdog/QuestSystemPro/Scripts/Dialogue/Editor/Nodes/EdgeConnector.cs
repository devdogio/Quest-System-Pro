using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class EdgeConnector
    {
        public int index;
        public int position;
        public Rect rect;
        public NodeEditorBase node;

        /// Is this a helper connectors that the user can use to add new edges. 
        public bool isHelperConnector;

        public EdgeConnector(int index, int position, Rect rect, bool isHelper, NodeEditorBase node)
        {
            this.index = index;
            this.position = position;
            this.rect = rect;
            this.isHelperConnector = isHelper;
            this.node = node;
        }


        //public static bool IsHelperConnector(int position)
        //{
        //    return position % 2 == 0;
        //}
    }
}
