using System;
using UnityEngine;
using System.Collections;
using Devdog.General.Editors;
using UnityEditor;
using EditorStyles = UnityEditor.EditorStyles;
using EditorUtility = UnityEditor.EditorUtility;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(WaypointGroup), true)]
    public class WaypointGroupEditor : Editor
    {
        private static WaypointGroup _hoveringWaypointGroup;
        private static int _hoveringWaypointIndex = -1;

        private static WaypointGroup _selectedWaypointGroup;
        private static int _selectedWaypointIndex = -1;

        private const float LineWidth = 3f;

        private ModuleList<IWaypointAction> _waypointActions;
        private ModuleList<IWaypointCondition> _waypointConditions;

        private static bool _holdingDeleteKey
        {
            get { return Event.current.control; }
        }

        public void OnEnable()
        {
            var t = (WaypointGroup)target;

            _waypointActions = new ModuleList<IWaypointAction>(t, this, "Global waypoint actions");
            _waypointActions.description = "Global waypoint actions are applied to all waypoints in this group.";

            _waypointConditions = new ModuleList<IWaypointCondition>(t, this, "Global waypoint conditions");
            _waypointConditions.description = "Global waypoint conditions are applied to all waypoints in this group.";
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Hold shift to add waypoints\nHold ctrl to delete waypoints", MessageType.Info);

            EditorGUILayout.LabelField("Waypoint Group Properties", EditorStyles.boldLabel);

            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            var group = (WaypointGroup)target;

            _waypointActions.DoLayoutList();
            _waypointConditions.DoLayoutList();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor Tools", EditorStyles.boldLabel);
            if (GUILayout.Button("Center pivot"))
            {
                Undo.RecordObject(group, "Centered Waypoints");
                group.CenterPivot();
            }
        }

        protected void OnSceneGUI()
        {
            var group = (WaypointGroup)target;

            Selection.activeObject = group;

            HandlePositionHandle(group);
            HandleInput(group);
        }

        private static void HandlePositionHandle(WaypointGroup group)
        {
            for (int i = 0; i < group.waypoints.Length; i++)
            {
                if (group != _selectedWaypointGroup || i != _selectedWaypointIndex)
                {
                    continue;
                }

                var waypoint = group.waypoints[i % group.waypoints.Length];
                var waypointWorldPosition = waypoint.transform.position;

                EditorGUI.BeginChangeCheck();
                waypointWorldPosition = Handles.PositionHandle(waypointWorldPosition, group.transform.rotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(group, "Moved waypoint");

                    waypoint.transform.localPosition = group.transform.worldToLocalMatrix * (waypointWorldPosition - group.transform.localPosition);
                    EditorUtility.SetDirty(group);
                    EditorUtility.SetDirty(group.waypoints[i].transform);
                }
            }
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        protected static void DrawWaypoints(WaypointGroup group, GizmoType type)
        {
            Handles.color = Color.yellow;
            for (int i = 0; i < group.waypoints.Length; i++)
            {
                var waypoint = group.waypoints[i % group.waypoints.Length];
                Waypoint nextWaypoint = null;
                if (group.loop)
                {
                    nextWaypoint = group.waypoints[(i + 1)%group.waypoints.Length];
                }
                else
                {
                    if (i + 1 < group.waypoints.Length)
                    {
                        nextWaypoint = group.waypoints[i + 1];
                    }
                }

                var waypointWorldPosition = waypoint.transform.position;
                if (group == _hoveringWaypointGroup && i == _hoveringWaypointIndex)
                {
                    if (_holdingDeleteKey)
                    {
                        Handles.color = Color.red;
                    }
                    else
                    {
                        Handles.color = Color.green;
                    }
                }

                if (group == _selectedWaypointGroup && i == _selectedWaypointIndex)
                {
                    Handles.color = Color.green;
                }

#if UNITY_2017_1_OR_NEWER
                Handles.SphereHandleCap(-1, waypointWorldPosition, Quaternion.identity, 0.4f, EventType.Repaint);
#else
                Handles.SphereCap(-1, waypointWorldPosition, Quaternion.identity, 0.4f);
#endif
                Handles.color = Color.yellow;

                if (nextWaypoint != null && group.waypoints.Length > 1)
                {
                    var waypointDirection = (waypoint.transform.localPosition - nextWaypoint.transform.localPosition);
                    waypointDirection = group.transform.localToWorldMatrix * waypointDirection;

                    var nextWaypointWorldPosition = nextWaypoint.transform.position;
                    Handles.DrawAAPolyLine(LineWidth, waypointWorldPosition, nextWaypointWorldPosition);

                    var midLine = waypoint.transform.localPosition + nextWaypoint.transform.localPosition;
                    midLine *= 0.5f;
                    midLine = ((Vector3)(group.transform.localToWorldMatrix * midLine)) + group.transform.localPosition;

                    // Draw an arrow to indicate the direction of this waypoint connection
                    var left = Quaternion.LookRotation(waypointDirection) * Quaternion.Euler(0f, 30f, 0f) * new Vector3(0f, 0f, 0.5f);
                    var right = Quaternion.LookRotation(waypointDirection) * Quaternion.Euler(0f, -30f, 0f) * new Vector3(0f, 0f, 0.5f);

                    Handles.DrawAAPolyLine(LineWidth, midLine, midLine + left);
                    Handles.DrawAAPolyLine(LineWidth, midLine, midLine + right);
                }
            }
            Handles.color = Color.white;
        }

        private static void HandleInput(WaypointGroup group)
        {
            var camera = SceneView.currentDrawingSceneView.camera;
            if (camera == null)
            {
                return;
            }

            _hoveringWaypointGroup = null;
            _hoveringWaypointIndex = 0;

            Vector2 mouseScreenPos = Event.current.mousePosition;
            mouseScreenPos.y = camera.pixelHeight - mouseScreenPos.y;

            if (group.waypoints.Length == 0)
            {
                AddWaypointToGroupOnClick(group, 0, GetWorldpsacePositionRaycastMouseScreenPos(camera));
                return;
            }

            for (int i = 0; i < group.waypoints.Length; i++)
            {
                var waypoint = group.waypoints[i % group.waypoints.Length];
                var nextWaypoint = group.waypoints[(i + 1) % group.waypoints.Length];

                var waypointWorldPosition = waypoint.transform.position;
                var nextWaypointWorldPosition = nextWaypoint.transform.position;

                Vector2 waypointScreenPos = camera.WorldToScreenPoint(waypointWorldPosition);
                Vector2 nextWaypointScreenPos = camera.WorldToScreenPoint(nextWaypointWorldPosition);

                if (Event.current.shift)
                {
                    // Get the screen position and see if the cursor is on a line between 2 waypoints.
                    // When clicked add a waypoint between the 2 set wayponits (insert in between).
                    // When holding ctrl - delete the waypoint the user is hovering over.
                    var screenSpaceDist = (waypointScreenPos - nextWaypointScreenPos).magnitude;
                    var waypointDirectionScreenSpaceDir = (waypointScreenPos - nextWaypointScreenPos) / screenSpaceDist;
                    var dot = Vector3.Dot(waypointDirectionScreenSpaceDir, (mouseScreenPos - waypointScreenPos).normalized);

                    var isCursorInBetweenWaypoints = mouseScreenPos.x > Mathf.Min(waypointScreenPos.x, nextWaypointScreenPos.x) &&
                                                     mouseScreenPos.x < Mathf.Max(waypointScreenPos.x, nextWaypointScreenPos.x) &&
                                                     mouseScreenPos.y > Mathf.Min(waypointScreenPos.y, nextWaypointScreenPos.y) &&
                                                     mouseScreenPos.y < Mathf.Max(waypointScreenPos.y, nextWaypointScreenPos.y);

                    if (Mathf.Abs(dot) > 0.99f && isCursorInBetweenWaypoints)
                    {
                        // Check the distance between the 2 points in screenspace and use this to check where the user clicked on the line.
                        var worldSpaceDist = (nextWaypointWorldPosition - waypointWorldPosition).magnitude;
                        var worldSpaceDir = (nextWaypointWorldPosition - waypointWorldPosition) / worldSpaceDist;
                        var distFactor = (mouseScreenPos - waypointScreenPos).magnitude / screenSpaceDist;
                        var position = waypointWorldPosition + (worldSpaceDir * distFactor * worldSpaceDist);

                        AddWaypointToGroupOnClick(group, i + 1, position);
                        return;
                    }
                    else
                    {
                        if (i == group.waypoints.Length - 1)
                        {
                            AddWaypointToGroupOnClick(group, group.waypoints.Length, GetWorldpsacePositionRaycastMouseScreenPos(camera));
                            return;
                        }
                    }
                }


                if ((waypointScreenPos - mouseScreenPos).sqrMagnitude < 900f)
                {
                    _hoveringWaypointGroup = group;
                    _hoveringWaypointIndex = i;

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (_holdingDeleteKey)
                        {
                            group.RemoveWaypoint(i);

                            _selectedWaypointGroup = null;
                            _selectedWaypointIndex = 0;
                        }
                        else
                        {
                            _selectedWaypointGroup = group;
                            _selectedWaypointIndex = i;
                        }
                    }

                    return;
                }
            }
        }

        private static Vector3 GetWorldpsacePositionRaycastMouseScreenPos(Camera camera)
        {
            Vector2 mouseScreenPos = Event.current.mousePosition;
            mouseScreenPos.y = camera.pixelHeight - mouseScreenPos.y;

            Ray ray = camera.ScreenPointToRay(mouseScreenPos);
            RaycastHit hitResult;
            if (Physics.Raycast(ray, out hitResult, float.PositiveInfinity, -1, QueryTriggerInteraction.Ignore))
            {
                return hitResult.point;
            }

            return Vector3.zero;
        }

        private static void AddWaypointToGroupOnClick(WaypointGroup group, int afterIndex, Vector3 worldPosition)
        {
            if (Event.current.alt)
            {
                // when using alt the user is likely panning around an object, ignore the add method.
                return;
            }

            Handles.Label(worldPosition, "Inserted waypoint at: " + worldPosition);
            Handles.color = Color.green;

#if UNITY_2017_1_OR_NEWER
            Handles.SphereHandleCap(-1, worldPosition, Quaternion.identity, 0.5f, EventType.Repaint);
#else
            Handles.SphereCap(-1, worldPosition, Quaternion.identity, 0.5f);
#endif

            var afterIndexClamped = Mathf.Max(0, afterIndex - 1);
            if (afterIndexClamped > 0 && afterIndexClamped < group.waypoints.Length)
            {
                Handles.DrawAAPolyLine(LineWidth, group.waypoints[afterIndexClamped].transform.position, worldPosition);
            }

            Handles.color = Color.white;

            // Connect from the last to a new waypoint
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Undo.RecordObject(group, "Added waypoint");
                group.InsertWaypointAfter(group.transform.worldToLocalMatrix * (worldPosition - group.transform.localPosition), afterIndex);

                _selectedWaypointGroup = group;
                _selectedWaypointIndex = afterIndex;

                EditorUtility.SetDirty(group);
            }
        }
    }
}