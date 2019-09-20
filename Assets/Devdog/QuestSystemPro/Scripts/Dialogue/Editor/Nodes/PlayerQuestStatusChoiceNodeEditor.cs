using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomNodeEditor(typeof(PlayerQuestStatusChoiceNode))]
    public class PlayerQuestStatusChoiceNodeEditor : DefaultNodeEditor
    {
        public override void DrawEdges()
        {
            base.DrawEdges();
        }

        protected override List<EdgeConditionSummary> GetEdgeConditionSummaries(uint edgeIndex)
        {
            var l = base.GetEdgeConditionSummaries(edgeIndex);
            var types = Enum.GetNames(typeof(QuestStatusAction));
            if (edgeIndex < types.Length)
            {
                l.Add(new EdgeConditionSummary()
                {
                    canUse = true,
                    color = Color.white,
                    msg = types[edgeIndex]
                });
            }

            return l;
        }

        protected override void DoDrawEdge(Edge edge, uint index, Vector2 from, Vector2 to, Color defaultColor)
        {
            var color = Color.grey;
            if (index == 0 || index == 1)
            {
                color = Color.green;
            }
            else if (index == 2)
            {
                color = Color.red;
            }

            if (editor.selectedEdges.Contains(edge))
            {
                DialogueEditorUtility.DrawCurves(from, to, color, 8f);
            }
            else
            {
                DialogueEditorUtility.DrawCurves(from, to, color);
            }
        }
    }
}
