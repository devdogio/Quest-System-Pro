using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Devdog.General.Editors.ReflectionDrawers;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomNodeEditor(typeof(PlayerDecisionNode))]
    public class PlayerDecisionNodeEditor : DefaultNodeEditor
    {
        protected override void DrawSingleField(ref Rect elementRect, DrawerBase drawer)
        {
            var n = ((PlayerDecisionNode)node);
            maxOutgoingEdges = n.playerDecisions.Length;

            if (ReferenceEquals(drawer.value, n.playerDecisions))
            {
                // TODO: Move to utility class -> Can reuse.
                if (drawer.fieldInfo.FieldType.IsArray)
                {
                    //var value = (Array)drawer.value;
                    var arrayDrawer = (ArrayDrawer) drawer;
                    var elem = arrayDrawer.children.FirstOrDefault();

                    var r = elementRect;
                    float height = ReflectionDrawerStyles.singleLineHeight;
                    if (elem != null)
                    {
                        height = elem.GetHeight();
                    }

                    r.height = height;
                }
            }

            base.DrawSingleField(ref elementRect, drawer);
        }
    }
}
