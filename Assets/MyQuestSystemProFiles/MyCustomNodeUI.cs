using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.UI;
using UnityEngine;

namespace Assets.MyQuestSystemProFiles
{
    public class MyCustomNodeUI : NodeUIBase
    {
        public override void Repaint(NodeBase node)
        {
            message.color = Color.red;
            base.Repaint(node);
        }
    }
}
