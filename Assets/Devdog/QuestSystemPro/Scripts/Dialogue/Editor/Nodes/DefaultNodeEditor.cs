using System;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.Editors;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;
using EditorStyles = UnityEditor.EditorStyles;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class DefaultNodeEditor : NodeEditorBase
    {
        public const NodeStyle DurrentNodeStyleKey = NodeStyle.Default;
        private Dictionary<NodeStyle, NodeStyleElement> _nodeStyles;
        protected Dictionary<NodeStyle, NodeStyleElement> nodeStyles
        {
            get
            {
                if (_nodeStyles == null)
                {
                    _nodeStyles = new Dictionary<NodeStyle, NodeStyleElement>()
                    {
                        { NodeStyle.Default, new NodeStyleElement("flow node 0", "flow node 0 on", Color.white) },
                        { NodeStyle.Blue, new NodeStyleElement("flow node 1", "flow node 1 on", Color.white) },
                        { NodeStyle.Green, new NodeStyleElement("flow node 3", "flow node 3 on", Color.white) },
                        { NodeStyle.Yellow, new NodeStyleElement("flow node 4", "flow node 4 on", Color.black) },
                        { NodeStyle.Orange, new NodeStyleElement("flow node 5", "flow node 5 on", Color.black) },
                        { NodeStyle.Red, new NodeStyleElement("flow node 6", "flow node 6 on", Color.white) },
                    };
                }

                return _nodeStyles;
            }
        }

        public override void Draw(bool drawContents)
        {
            try
            {
                DrawNodeState();

                var style = nodeStyles[node.editorNodeStyle];
                GUI.contentColor = style.contentColor;

                using (new GroupBlock(GetNodeRect(), GUIContent.none, isSelected ? style.active : style.normal))
                {
                    var elementRect = new Rect(NodeInnerPadding, NodeInnerPadding, nodeSize.x - NodeInnerPadding * 2f, ReflectionDrawerStyles.singleLineHeight);
                    GUI.Label(elementRect, "#" + node.index + " " + UnityEditor.ObjectNames.NicifyVariableName(node.GetType().Name), EditorStyles.boldLabel);

                    if (drawContents)
                    {
                        elementRect.y += ReflectionDrawerStyles.singleLineHeight;
                        DrawFields(ref elementRect);
                    }
                }

                GUI.contentColor = Color.white;

                DrawDebugView();
                DrawNodeValidation(node.Validate());

                if (drawContents)
                {
                    DrawEdgeConnectors();
                    DrawReceivingEdgeConnectors();
                }
            }
            catch (Exception e)
            {
                DrawValidation(ValidationType.Error, e.Message);
                //throw; // Ignored, displayed in UI
            }
        }

        protected virtual void DrawFields(ref Rect elementRect)
        {
            int i = 0;
            float height = ReflectionDrawerStyles.singleLineHeight + NodeInnerPadding * 2;

            foreach (var drawer in drawers)
            {
                if (ShouldShowDrawerInNode(drawer))
                {
                    continue;
                }

                DrawSingleField(ref elementRect, drawer);
                height += drawer.GetHeight();

                i++;
            }
            
            // Make sure we can never have more edges than specified -> Delete if necesary...
            if (node.edges.Length > maxOutgoingEdges)
            {
                var l = node.edges.ToList();
                while (l.Count > maxOutgoingEdges)
                {
                    l.RemoveAt(l.Count - 1);
                }

                node.edges = l.ToArray();
            }

            this.nodeSize.y = height;
        }

        protected virtual void DrawSingleField(ref Rect elementRect, DrawerBase drawer)
        {
            EditorGUI.BeginChangeCheck();

            drawer.Draw(ref elementRect);

            if(EditorGUI.EndChangeCheck())
            {
                NotifyFieldChanged(drawer);
            }
        }

        protected virtual void NotifyFieldChanged(DrawerBase drawer)
        {

        }

        public override void DrawSidebar(ref Rect rect)
        {
            foreach (var drawer in drawers)
            {
                if (drawer.hideInProperties)
                {
                    continue;
                }

                DrawSingleField(ref rect, drawer);
            }
        }
    }
}
