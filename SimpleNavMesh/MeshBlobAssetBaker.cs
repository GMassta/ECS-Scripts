using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsScripts.SimpleNavMesh
{
    public struct MeshData
    {
        public BlobArray<float3> vertices;
        public BlobArray<int> triangles;
    }
    
    public static class MeshBlobAssetBaker
    {
        public static BlobAssetReference<MeshData> CreateMeshBlob(Mesh mesh)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var data = ref builder.ConstructRoot<MeshData>();

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            var vArray = builder.Allocate(ref data.vertices, vertices.Length);
            for (int i = 0, size = vArray.Length; i < size; i++)
                vArray[i] = vertices[i];

            var tArray = builder.Allocate(ref data.triangles, triangles.Length);
            for (int i = 0, size = tArray.Length; i < size; i++)
                tArray[i] = triangles[i];

            var result = builder.CreateBlobAssetReference<MeshData>(Allocator.Persistent);
            builder.Dispose();

            return result;
        }
    }
}