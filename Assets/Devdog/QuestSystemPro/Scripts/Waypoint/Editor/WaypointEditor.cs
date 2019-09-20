using System;
using UnityEngine;
using Devdog.General.Editors;
using UnityEditor;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(Waypoint), true)]
    public class WaypointEditor : Editor
    {
        private ModuleList<IWaypointAction> _waypointActions;
        private ModuleList<IWaypointCondition> _waypointConditions;

        public void OnEnable()
        {
            var t = (Waypoint)target;

            _waypointActions = new ModuleList<IWaypointAction>(t, this, "Waypoint actions");
            _waypointActions.description = "Waypoint actions can be used to let a character perform an action at this waypoing\nThese actions can be async.";
            
            _waypointConditions = new ModuleList<IWaypointCondition>(t, this, "Waypoint conditions");
            _waypointConditions.description = "Waypoint conditions can be used to prevent a character from moving to the next waypoint.";
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            DrawPropertiesExcluding(serializedObject, "m_Script");

            _waypointActions.DoLayoutList();
            _waypointConditions.DoLayoutList();
        }
    }
}