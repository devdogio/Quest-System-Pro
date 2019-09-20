using Devdog.General;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(WeightedTextureSpawnerVolume), true)]
    public class WeightedTextureSpawnerVolumeEditor : Editor
    {
        protected static Material material;
        protected static Mesh planeMesh;

        protected const string TexturePreviewShader = "Devdog/WeightedTexturePreview";
        protected const string TextureChannelUniform = "_Channel";

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmos(WeightedTextureSpawnerVolume volume, GizmoType type)
        {
            if (planeMesh == null)
            {
                planeMesh = GeneratePlaneMesh();
            }

            if (material == null)
            {
                material = new Material(Shader.Find(TexturePreviewShader));
            }

            var matrix = Matrix4x4.TRS(volume.transform.position + Vector3.up, volume.transform.rotation, new Vector3(volume.volumeSize.x, 30f, volume.volumeSize.y));

            material.SetTexture("_MainTex", volume.distributionTexture);
            material.SetInt(TextureChannelUniform, (int)volume.channel);
            material.SetPass(0);
            Graphics.DrawMeshNow(planeMesh, matrix, 0);

            DrawVolumeAABB(volume);
        }

        protected static void DrawVolumeAABB(WeightedTextureSpawnerVolume volume)
        {
            var spawner = volume.GetComponent<SpawnerBase>();
            float height = 30f;
            if (spawner != null && spawner.spawnerInfo.maxRaycastDistance > 0f)
            {
                height = spawner.spawnerInfo.maxRaycastDistance;
            }

            var matrix = Matrix4x4.TRS(volume.transform.position + Vector3.up, volume.transform.rotation, new Vector3(volume.volumeSize.x, height, volume.volumeSize.y));
            var before = Gizmos.matrix;
            Gizmos.matrix = matrix;

            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = Color.white;

            Gizmos.matrix = before;
        }

        protected static Mesh GeneratePlaneMesh()
        {
            var mesh = new Mesh();
            var vertices = new Vector3[]
            {
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(-0.5f, 0, 0.5f),
                new Vector3(-0.5f, 0, -0.5f),
            };

            var uv = new Vector2[]
            {
                new Vector2(1f, 1f),
                new Vector2(1f, 0),
                new Vector2(0, 1f),
                new Vector2(0, 0),
            };

            var triangles = new int[]
            {
                0, 1, 2,
                2, 1, 3,
            };

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
