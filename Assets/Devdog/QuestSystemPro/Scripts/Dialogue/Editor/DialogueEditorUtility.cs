using System;
using UnityEngine;
using UnityEditor;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public static class DialogueEditorUtility
    {

        public static void DrawCurves(Vector2 startPos, Vector2 endPos, Color color, float size = 3f)
        {
            float dist = Vector3.Distance(startPos, endPos);
            dist = Mathf.Min(dist, 200f);

            Vector3 startTangent = startPos + Vector2.up * (dist / 3f);
            Vector3 endTangent = endPos + Vector2.down * (dist / 3f);

            Handles.BeginGUI();
            Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, size);
            Handles.EndGUI();
        }

    }
}