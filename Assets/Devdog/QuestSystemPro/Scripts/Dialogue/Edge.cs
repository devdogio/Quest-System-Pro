using System;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    public class Edge
    {
        [InspectorReadOnly]
        public uint toNodeIndex;

        [HideGroup(false)]
        public IEdgeCondition[] conditions;

        
//        [Obsolete("Don't use directly, use onther constructor(s) instead.", true)]
        public Edge()
            : this(0)
        { }

        public Edge(uint toNodeIndex)
        {
            this.toNodeIndex = toNodeIndex;
            if (conditions == null)
            {
                conditions = new IEdgeCondition[0];
            }
        }

        public bool CanViewEndNode(Dialogue dialogue)
        {
            return conditions.All(o => o.CanViewEndNode(dialogue)) && dialogue.nodes[toNodeIndex].CanViewNode();
        }

        public bool CanUse(Dialogue dialogue)
        {
            return conditions.All(o => o.CanUse(dialogue)) && dialogue.nodes[toNodeIndex].CanUseNode();
        }
    }
}