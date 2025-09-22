using System;
using _Content.Scripts.Utils;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsScripts.SimpleNavMesh
{
    public class RoadAuthoring : MonoBehaviour
    {
        [Serializable]
        private struct RoadSegment
        {
            public Vector3 position;
            public float angle;
            public float width;
        }

        [SerializeField] private RoadSegment[] segments;
        
        private class RoadAuthoringBaker : Baker<RoadAuthoring>
        {
            public override void Bake(RoadAuthoring authoring)
            {
                var mesh = authoring.GetMesh();
                var blob = MeshBlobAssetBaker.CreateMeshBlob(mesh);

                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new NavMeshSurface {data = blob});
            }
        }

        private void OnDrawGizmos()
        {
            var mesh = GetMesh();
            UnityEngine.Gizmos.color = new Color(0.2f, .64f, .5f, .4f);
            UnityEngine.Gizmos.DrawMesh(mesh);
        }

        private Mesh GetMesh()
        {
            if (segments.Length < 2) return null;
            var length = segments.Length;
            
            var mesh = new Mesh();
            var vertices = new Vector3[4 * length];
            var normals = new Vector3[4 * length];
            var triangles = new int[6 * length];

            for (var i = 0; i < segments.Length - 1; i++)
            {
                var s1 = segments[i];
                var s2 = segments[i + 1];

                var p1 = s1.position;
                var p2 = s2.position;

                var r1 = Quaternion.Euler(0, s1.angle, 0);
                var r2 = Quaternion.Euler(0, s2.angle, 0);

                var index = i * 4;
                vertices[index] = p1 + r1 * Vector3.left * s1.width;
                vertices[index + 1] = p1 + r1 * Vector3.right * s1.width;
                vertices[index + 2] = p2 + r2 * Vector3.right * s2.width;
                vertices[index + 3] = p2 + r2 * Vector3.left * s2.width;

                var tIndex = i * 6;
                triangles[tIndex] = index;
                triangles[tIndex + 1] = index + 3;
                triangles[tIndex + 2] = index + 2;
                triangles[tIndex + 3] = index;
                triangles[tIndex + 4] = index + 2;
                triangles[tIndex + 5] = index + 1;

                normals[index] = Vector3.up;
                normals[index + 1] = Vector3.up;
                normals[index + 2] = Vector3.up;
                normals[index + 3] = Vector3.up;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.RecalculateBounds();
            return mesh;
        }
    }

    public struct NavMeshSurface : IComponentData
    {
        public BlobAssetReference<MeshData> data;
    }

    public struct CheckPointOnSurface : IComponentData
    {
        public float3 checkPoint;
        public float3 foundPoint;
        public bool onBoard;
    }
}