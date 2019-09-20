using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Devdog.General;
using Devdog.General.Editors.ReflectionDrawers;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class DialogueEditorSidebarProperties : DialogueEditorSidebarBase
    {
        private DialogueEditorWindow _editor;

        private NodeEditorBase _nodeEditor;
        private Edge _edge;

        private List<DrawerBase> _edgeDrawers = new List<DrawerBase>();

        public DialogueEditorSidebarProperties(string name)
            : base(name)
        {


        }

        public void Update(DialogueEditorWindow editor)
        {
            _editor = editor;
            _nodeEditor = editor.selectedNodeEditors.FirstOrDefault();
            _edge = editor.selectedEdges.FirstOrDefault();
            _edgeDrawers.Clear();

            if (_edge != null)
            {
                _edgeDrawers = ReflectionDrawerUtility.BuildEditorHierarchy(_edge.GetType(), _edge).ToList();
            }
        }

        public override void Draw(Rect rect, DialogueEditorWindow editor)
        {
            if ((_editor != null && _editor.dialogue != editor.dialogue) ||
                (editor.selectedNodeEditors.Count > 0 && editor.selectedNodeEditors.Contains(_nodeEditor) == false) ||
                (editor.selectedEdges.Count > 0 && editor.selectedEdges.Contains(_edge) == false))
            {
                Update(editor);
            }

            if (editor.selectedNodeEditors.Count == 0 && editor.selectedEdges.Count == 0)
            {
                GUI.Label(rect, "Select a node or edge (line) to edit it.");
                rect.y += EditorGUIUtility.singleLineHeight;
            }

            DrawEdgeEditor(ref rect, editor);
            DrawNodeEditor(ref rect, editor);
        }

        private void DrawNodeEditor(ref Rect rect, DialogueEditorWindow editor)
        {
            var a = editor.selectedNodeEditors.FirstOrDefault();
            if (a != null)
            {
                a.DrawSidebar(ref rect);
            }
        }

        private void DrawEdgeEditor(ref Rect rect, DialogueEditorWindow editor)
        {
            if (editor.selectedEdges.Count > 0)
            {
                foreach (var edgeDrawer in _edgeDrawers)
                {
                    edgeDrawer.Draw(ref rect);
                }
            }
        }
    }
}
