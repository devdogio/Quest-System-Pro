using System;
using UnityEngine;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using System.Linq;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomNodeEditor(typeof(SetVariableNode<>))]
    public class SetVariableNodeEditor : DefaultNodeEditor
    {
        private static DerivedTypeInformation _types;
        private int _typeIndex;

        public override void Init(NodeBase node, DialogueEditorWindow editor)
        {
            base.Init(node, editor);

            if (_types == null)
            {
                _types = ReflectionDrawerUtility.GetDerivedTypesFrom(typeof(SetVariableNode<>), null);
            }

            _typeIndex = Mathf.Max(Array.IndexOf(_types.types, node.GetType()), 0);
        }

        protected override void DrawFields(ref Rect elementRect)
        {
            base.DrawFields(ref elementRect);

            EditorGUI.BeginChangeCheck();
            _typeIndex = EditorGUI.Popup(elementRect, new GUIContent("Variable type"), _typeIndex, _types.content);
            if(EditorGUI.EndChangeCheck())
            {
                // Changed, replace this node editor for a new instance of the new generic type...
                var nodeIndexInDialogue = Array.IndexOf(editor.dialogue.nodes, node);
                if(nodeIndexInDialogue == -1)
                {
                    Debug.LogError("Couldn't find node to change in current dialogue!!");
                    return;
                }

                var editorIndex = Array.IndexOf(DialogueEditorWindow.window.nodeEditors.ToArray(), this);
                if (editorIndex == -1)
                {
                    Debug.LogError("Couldn't find node editor to change in current dialogue editor!!");
                    return;
                }

                // Do the node
                {
                    var oldNode = editor.dialogue.nodes[nodeIndexInDialogue];
                    var newNode = NodeFactory.Create(_types.types[_typeIndex], node.edges);

                    newNode.index = oldNode.index;
                    newNode.owner = oldNode.owner;
                    newNode.ownerType = oldNode.ownerType;
                    newNode.useAutoFocus = oldNode.useAutoFocus;
                    newNode.audioInfo = oldNode.audioInfo;
                    newNode.motionInfo = oldNode.motionInfo;
                    newNode.editorPosition = oldNode.editorPosition;

                    node = newNode;
                    editor.dialogue.nodes[nodeIndexInDialogue] = newNode;
                }

                // Do the node editor
                {
                    editor.nodeEditors[editorIndex] = editor.CreateEditorForNode(node);

                    EditorUtility.SetDirty(editor.dialogue);
                    GUI.changed = true;

                    editor.Repaint();
                }
            }

            nodeSize.y = GetNodeRect().height;
            nodeSize.y += ReflectionDrawerStyles.singleLineHeight;
        }
    }
}
