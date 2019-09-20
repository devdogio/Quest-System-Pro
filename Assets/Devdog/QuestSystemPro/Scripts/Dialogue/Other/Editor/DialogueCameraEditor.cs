using UnityEngine;
using System.Collections;
using System.Linq;
using Devdog.QuestSystemPro.Dialogue;
using UnityEditor;
using System;
using UnityEditorInternal;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    [CustomEditor(typeof(DialogueCamera), true)]
    public class DialogueCameraEditor : Editor
    {
        protected SerializedProperty lookups;

        protected ReorderableList list;
        protected static int focusedIndex = -1;

        protected virtual void OnEnable()
        {
            lookups = serializedObject.FindProperty("lookups");

            var t = (DialogueCamera)target;
            const int lineHeight = 18;

            list = new ReorderableList(serializedObject, lookups);
            list.elementHeight = lineHeight * 6 + 10; // + some padding
            list.draggable = false;
            list.drawHeaderCallback = delegate(Rect rect)
            {
                EditorGUI.LabelField(rect, "Camera positions");
            };
            list.drawElementCallback = delegate(Rect rect, int index, bool active, bool focused)
            {
                var element = lookups.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 2;


                if (focused)
                {
                    focusedIndex = index;
                }

                var key = element.FindPropertyRelative("key");
                var position = element.FindPropertyRelative("position");
                var rotation = element.FindPropertyRelative("rotation");
                var useWorldSpace = element.FindPropertyRelative("useWorldSpace");

                EditorGUI.PropertyField(rect, key);
                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, position);
                rect.y += lineHeight;

                var eulerAnglesRot = EditorGUI.Vector3Field(rect, rotation.displayName, rotation.quaternionValue.eulerAngles);
                rotation.quaternionValue = Quaternion.Euler(Round(eulerAnglesRot));

                rect.y += lineHeight;
                EditorGUI.PropertyField(rect, useWorldSpace);
                rect.y += lineHeight;

                rect.width /= 2f;
                if (GUI.Button(rect, "Copy from transform"))
                {
                    if (useWorldSpace.boolValue)
                    {
                        position.vector3Value = t.transform.position;
                        rotation.quaternionValue = t.transform.rotation;
                    }
                    else
                    {
                        position.vector3Value = t.transform.localPosition;
                        rotation.quaternionValue = t.transform.localRotation;
                    }
                }

                rect.x += rect.width;
                if(GUI.Button(rect, "Copy from editor camera"))
                {
                    var editorCamTransform = SceneView.lastActiveSceneView.camera.transform;

                    if (useWorldSpace.boolValue)
                    {
                        position.vector3Value = editorCamTransform.position;
                        rotation.quaternionValue = editorCamTransform.rotation;
                    }
                    else
                    {
                        position.vector3Value = t.transform.InverseTransformPoint(editorCamTransform.position);
                        rotation.quaternionValue = t.transform.rotation * editorCamTransform.transform.rotation;
                    }
                }

                rect.x -= rect.width;
                rect.width *= 2;
                rect.y += lineHeight;
                if (GUI.Button(rect, "Move to position"))
                {
                    if (useWorldSpace.boolValue)
                    {
                        SceneView.lastActiveSceneView.pivot = position.vector3Value;
                        SceneView.lastActiveSceneView.rotation = rotation.quaternionValue;
                    }
                    else
                    {
                        SceneView.lastActiveSceneView.pivot = t.transform.TransformPoint(position.vector3Value);
                        SceneView.lastActiveSceneView.rotation = t.transform.rotation * rotation.quaternionValue;
                    }

                    SceneView.lastActiveSceneView.Repaint();
                }
            };
        }

        public Vector3 Round(Vector3 v)
        {
            v.x = (float)Math.Round(v.x, 2);
            v.y = (float)Math.Round(v.y, 2);
            v.z = (float)Math.Round(v.z, 2);

            return v;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            focusedIndex = -1;
            EditorGUILayout.Space();
            list.DoLayoutList();

            DrawPropertiesExcluding(serializedObject, lookups.name, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(DialogueCamera camera, GizmoType gizmoType)
        {
            for (int i = 0; i < camera.lookups.Length; i++)
            {
                var lookup = camera.lookups[i];
                var c = camera.GetComponent<Camera>();
                var tempMatrix = Gizmos.matrix;

                if (focusedIndex == i)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.gray;
                }

                if (lookup.useWorldSpace)
                {
                    Handles.Label(lookup.position, lookup.key);

                    Gizmos.matrix = Matrix4x4.TRS(lookup.position, lookup.rotation, Vector3.one);
                    Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView, 50, 0.5f, c.aspect);
                }
                else
                {
                    Handles.Label(camera.transform.localToWorldMatrix * lookup.position, lookup.key);

                    Gizmos.matrix = Matrix4x4.TRS(lookup.position, lookup.rotation, Vector3.one);
                    Gizmos.matrix = camera.transform.localToWorldMatrix * Gizmos.matrix;

                    Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView, 50, 0.5f, c.aspect);
                }

                camera.lookups[i] = lookup; // Set it again (struct)
                Gizmos.color = Color.white;
                Gizmos.matrix = tempMatrix;
            }
        }
    }
}