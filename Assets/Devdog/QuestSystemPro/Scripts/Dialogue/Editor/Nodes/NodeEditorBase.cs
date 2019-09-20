using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Devdog.General;
using Devdog.General.Editors;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;
using EditorStyles = UnityEditor.EditorStyles;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public abstract class NodeEditorBase
    {
        public NodeBase node { get; protected set; }
        public DialogueEditorWindow editor { get; protected set; }
        public Vector2 nodeSize = new Vector2(200f, 100f);
        public GUIStyle edgeConnectorStyle
        {
            get { return (GUIStyle) "HelpBox"; }
        }

        private GUIStyle _edgeConnectorHelperStyle;
        public GUIStyle edgeConnectorHelperStyle
        {
            get
            {
                if(_edgeConnectorHelperStyle == null)
                {
                    _edgeConnectorHelperStyle = new GUIStyle((GUIStyle)"HelpBox");
                    _edgeConnectorHelperStyle.normal.textColor = new Color(0f, 0.5f, 0f);
                    _edgeConnectorHelperStyle.contentOffset = Vector2.zero;
                    _edgeConnectorHelperStyle.padding = new RectOffset(4, 4, 0, 0);
                    _edgeConnectorHelperStyle.margin = new RectOffset(0, 0, 0, 0);
                    _edgeConnectorHelperStyle.fontSize = 12;
                }

                return _edgeConnectorHelperStyle;
            }
        }

        public bool isSelected { get; protected set; }
        
        public const int NodeInnerPadding = 10;
        public const int EdgeConnectorSize = 18;

        public const float DebugViewFadeOutTime = 1f;
        private double _lastDebugViewTime = 0f;
        private DebugNodeVisualType _lastDebugDebugType;


        private Dictionary<DrawerBase, bool> _showInNodeDict = new Dictionary<DrawerBase, bool>();
        private readonly List<FieldInfo> _childFields = new List<FieldInfo>();
        protected List<DrawerBase> drawers = new List<DrawerBase>(0);

        protected int maxOutgoingEdges = 999;

        public virtual bool canDrag
        {
            get { return true; }
        }

        public virtual bool canSelect
        {
            get { return true; }
        }

        public virtual void Init(NodeBase node, DialogueEditorWindow editor)
        {
            this.node = node;
            this.editor = editor;

            _childFields.Clear();

            drawers = ReflectionDrawerUtility.BuildEditorHierarchy(node.GetType(), node);
            foreach (var drawer in drawers)
            {
                _showInNodeDict[drawer] = drawer.fieldInfo.GetCustomAttributes(typeof(ShowInNodeAttribute), true).Length == 0;
            }
        }

        protected bool ShouldShowDrawerInNode(DrawerBase drawer)
        {
            if (_showInNodeDict.ContainsKey(drawer) == false)
            {
                return false;
            }

            return _showInNodeDict[drawer];
        }

        public virtual bool CanDelete()
        {
            return true;
        }
        
        public virtual void Select()
        {
            isSelected = true;
        }

        public virtual void UnSelect()
        {
            isSelected = false;
        }

        public virtual void Drag(Vector2 drag)
        {
            node.editorPosition += drag;
        }

        public virtual EdgeConnector[] GetEdgeConnectors()
        {
            var l = new EdgeConnector[Mathf.Min(node.edges.Length * 2 + 1, maxOutgoingEdges == node.edges.Length ? maxOutgoingEdges : node.edges.Length * 2 + 1)];

            var r = new Rect(node.editorPosition.x + nodeSize.x / 2f - EdgeConnectorSize / 2f * l.Length, node.editorPosition.y + nodeSize.y, EdgeConnectorSize, EdgeConnectorSize);
            r.position += editor.nodesOffset;
            int index = 0;
            for (int i = 0; i < l.Length; i++)
            {
                var isHelper = i % 2 == 0;
                if ((maxOutgoingEdges == node.edges.Length || isHelper) && i > 0)
                {
                    index++;
                }

                var con = new EdgeConnector(index, i, r, isHelper && maxOutgoingEdges != node.edges.Length, this);
                l[i] = con;
                r.x += EdgeConnectorSize;
            }

            return l;
        }

        public virtual EdgeConnector[] GetReceivingEdgeConnectors()
        {
            var l = new EdgeConnector[]
            {
                new EdgeConnector(0, 0, new Rect(node.editorPosition.x + nodeSize.x / 2f - EdgeConnectorSize / 2f + editor.nodesOffset.x, node.editorPosition.y - EdgeConnectorSize + editor.nodesOffset.y, EdgeConnectorSize, EdgeConnectorSize), false, this)
            };

            return l;
        }

        protected virtual void DrawEdgeConnectors()
        {
            var connectors = GetEdgeConnectors();
            if(maxOutgoingEdges == node.edges.Length)
            {
                foreach (var connector in connectors)
                {
                    EditorGUIUtility.AddCursorRect(connector.rect, MouseCursor.ArrowPlus);
                    GUI.Button(connector.rect, "", edgeConnectorStyle);
                }
            }
            else
            {
                foreach (var connector in connectors)
                {
                    //// When we're at the max amount of outgoing edges stop drawing the helper connectors.
                    //if (node.edges.Length >= maxOutgoingEdges && connector.isHelperConnector)
                    //{
                    //    continue;
                    //}

                    EditorGUIUtility.AddCursorRect(connector.rect, MouseCursor.ArrowPlus);
                    if (connector.isHelperConnector)
                    {
                        GUI.color = Color.green;
                        GUI.Button(connector.rect, "+", edgeConnectorHelperStyle);
                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.Button(connector.rect, "", edgeConnectorStyle);
                    }
                }
            }
        }

        protected virtual void DrawReceivingEdgeConnectors()
        {
            foreach (var connector in GetReceivingEdgeConnectors())
            {
                EditorGUIUtility.AddCursorRect(connector.rect, MouseCursor.ArrowPlus);
                GUI.Button(connector.rect, "", edgeConnectorStyle);
            }
        }

        protected virtual void DrawNodeState()
        {
            if(editor.dialogue != null && editor.dialogue.currentNodeIndex == node.index)
            {
                GUI.color = Color.yellow;

                GUI.Label(GetNodeRect(), GUIContent.none, "LightmapEditorSelectedHighlight");

                GUI.color = Color.white;
            }
        }

        protected void DrawNodeValidation(ValidationInfo validation)
        {
            DrawValidation(validation.validationType, validation.message);
        }

        protected virtual void DrawValidation(ValidationType validationType, string message)
        {
            var r = GetNodeRect();

            r.x += r.width - 18;
            r.y -= 14;

            r.width = 30;
            r.height = 30;

            if (validationType == ValidationType.Warning)
            {
                EditorGUI.LabelField(r, GUIContent.none, "CN EntryWarn");
            }
            else if (validationType == ValidationType.Error)
            {
                EditorGUI.LabelField(r, GUIContent.none, "CN EntryError");
            }

            if (r.Contains(Event.current.mousePosition))
            {
                DrawTooltip(Event.current.mousePosition, message);
            }
        }

        private void DrawTooltip(Vector2 position, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var style = (GUIStyle)"SelectionRect";
            var height = 100f;

            GUI.color = new Color(1, 1, 1, 0.5f);
            using (new GroupBlock(new Rect(position.x, position.y, 240, height), GUIContent.none, style))
            {
                GUI.color = Color.white;
                EditorGUI.LabelField(new Rect(10, 10, 220, height - 20), message, EditorStyles.wordWrappedLabel);
            }

            GUI.color = Color.white;
        }

        protected virtual void DrawDebugView()
        {
            var diff = (float) (EditorApplication.timeSinceStartup - _lastDebugViewTime) - 0.2f;
            GUI.color = Color.Lerp(GetColorFromDebugNodeType(_lastDebugDebugType), Color.clear, diff);

            if (diff < 1.2f)
            {
                GUI.BeginGroup(GetNodeRect(), GUIContent.none, "LightmapEditorSelectedHighlight");
                GUI.EndGroup();
            }

            GUI.color = Color.white;
        }

        private Color GetColorFromDebugNodeType(DebugNodeVisualType type)
        {
            switch (type)
            {
                case DebugNodeVisualType.Debug:
                    return Color.cyan;
                case DebugNodeVisualType.Warning:
                    return Color.yellow;
                case DebugNodeVisualType.Error:
                    return Color.red;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        public virtual void PingDebugView(DebugNodeVisualType type)
        {
            _lastDebugViewTime = EditorApplication.timeSinceStartup;
            _lastDebugDebugType = type;
        }

        public virtual Rect GetNodeRect()
        {
            return new Rect(node.editorPosition + editor.nodesOffset, nodeSize);
        }

        public abstract void Draw(bool drawContents);
        public abstract void DrawSidebar(ref Rect rect);

        public virtual void DrawEdges()
        {
            if (node.edges.Any(o => o.toNodeIndex >= node.owner.nodes.Length))
            {
                var e = node.edges.ToList();
                e.RemoveAll(o => o.toNodeIndex >= node.owner.nodes.Length);
                node.edges = e.ToArray();
            }

            var connectors = GetEdgeConnectors();
            if (maxOutgoingEdges == node.edges.Length)
            {
                for (int i = 0; i < node.edges.Length; i++)
                {
                    DrawEdge((uint)connectors[i].index, connectors[i].rect);
                }
            }
            else
            {
                for (int i = 1; i < node.edges.Length * 2 + 1; i += 2)
                {
                    if (i >= connectors.Length)
                    {
                        break;
                    }

                    DrawEdge((uint)connectors[i].index, connectors[i].rect);
                }
            }
        }

        protected virtual void DrawEdge(uint i, Rect fromRect)
        {
            if (i >= node.edges.Length)
            {
                Debug.LogWarning("Out of range : total of " + node.edges.Length + " edges - index: " + i + " node type + " + node.GetType() + " - Node index: " + node.index);
                return; // Something is out of range!!
            }

            var edge = node.edges[i];
            var fromPos = fromRect.position;
            fromPos.x += fromRect.width / 2;
            fromPos.y += fromRect.height / 2;

            var to = editor.nodeEditors[(int)edge.toNodeIndex];
            var connector = to.GetReceivingEdgeConnectors()[0];
            var toPos = connector.rect.position;
            toPos.x += connector.rect.width / 2;
            toPos.y += connector.rect.height / 2;

            DoDrawEdge(edge, i, fromPos, toPos, Color.grey);
            DrawEdgeConditionSummaries(i, fromRect.position, connector.rect.position);
        }

        protected virtual void DoDrawEdge(Edge edge, uint index, Vector2 from, Vector2 to, Color defaultColor)
        {
            if (editor.selectedEdges.Contains(edge))
            {
                DialogueEditorUtility.DrawCurves(from, to, Color.yellow, 5f);
            }
            else
            {
                DialogueEditorUtility.DrawCurves(from, to, defaultColor);
            }
        }

        protected void DrawEdgeConditionSummaries(uint edgeIndex, Vector2 from, Vector2 to)
        {
            var summaries = GetEdgeConditionSummaries(edgeIndex);
            if (summaries.Count == 0)
            {
                return;
            }

            DoDrawEdgeSummary(summaries, GetSummaryRect(summaries, from, to));
        }

        protected virtual List<EdgeConditionSummary> GetEdgeConditionSummaries(uint edgeIndex)
        {
            var l = new List<EdgeConditionSummary>();

            var edge = node.edges[edgeIndex];
            foreach (var condition in edge.conditions)
            {
                var canUse = EditorApplication.isPlaying ? condition.CanUse(editor.dialogue) : true;

                l.Add(new EdgeConditionSummary()
                {
                    color = canUse || EditorApplication.isPlaying == false ? Color.white : Color.red,
                    msg = condition.FormattedString(),
                    canUse = canUse
                });
            }

            return l;
        }

        protected virtual Rect GetSummaryRect(IEnumerable<EdgeConditionSummary> msg, Vector2 from, Vector2 to)
        {
            int lines = msg.Count();

            float textWidth = 0f;
            foreach (var condition in msg)
            {
                var textWidthTemp = (int)GUI.skin.label.CalcSize(new GUIContent(condition.msg)).x;
                if (textWidthTemp > textWidth)
                {
                    textWidth = textWidthTemp;
                }
            }
            textWidth += 8f; // Some padding

            var summaryRect = new Rect
            {
                x = (from.x + to.x) / 2, //  - (textWidth / 2)
                y = (from.y + to.y) / 2,
                width = textWidth,
                height = (lines*EditorGUIUtility.singleLineHeight) + 6
            };

            summaryRect.x -= summaryRect.width/2;
            summaryRect.y -= summaryRect.height/2;

            summaryRect.x = Mathf.RoundToInt(summaryRect.x);
            summaryRect.y = Mathf.RoundToInt(summaryRect.y);
            return summaryRect;
        }

        protected void DoDrawEdgeSummary(IEnumerable<EdgeConditionSummary> msg, Rect rect)
        {
            var canUseAll = msg.All(o => o.canUse);
            GUIStyle style = "U2D.createRect";
            if (EditorApplication.isPlaying == false)
            {
                style = "SelectionRect";
            }
            else
            {
                if (canUseAll == false)
                {
                    GUI.color = Color.red;
                }
            }


            GUI.BeginGroup(rect, GUIContent.none, style);
            rect.x = 0;
            rect.y = 0;

            GUI.BeginGroup(rect, GUIContent.none, style);
            GUI.BeginGroup(rect, GUIContent.none, style);
            GUI.color = Color.white;

            rect.y += 2;
            rect.x += 3;
            rect.height = EditorGUIUtility.singleLineHeight;

            foreach (var condition in msg)
            {
                if (condition == null)
                {
                    continue;
                }

                GUI.color = condition.color;

                EditorGUI.LabelField(rect, condition.msg);
                rect.y += EditorGUIUtility.singleLineHeight;

                GUI.color = Color.white;
            }

            GUI.EndGroup();
            GUI.EndGroup();
            GUI.EndGroup();
        }

        public void SetEdge(EdgeConnector connector, NodeBase connectToNode)
        {
//            Debug.Log("Connect " + node.index + " to " + connectToNode.index);
            if (connector.isHelperConnector == false && connector.index <= node.edges.Length - 1)
            {
                var existingNode = node.edges[connector.index];
                if (existingNode != null)
                {
                    existingNode.toNodeIndex = connectToNode.index;
                }
            }
            else
            {
                var l = this.node.edges.ToList();
                l.Insert(connector.index, new Edge(connectToNode.index));

                this.node.edges = l.ToArray();
            }
        }

        public void RemoveEdge(uint index)
        {
            if (index <= this.node.edges.Length - 1)
            {
                var l = this.node.edges.ToList();
                l.RemoveAt((int) index);
                this.node.edges = l.ToArray();
            }
        }

        public bool CanConnectEdgeFromNode(NodeBase node)
        {
            return true;
        }
    }
}
