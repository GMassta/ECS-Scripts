using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace EcsScripts.SimpleNavMesh
{
    public partial struct SimpleNavMeshDrawSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NavMeshSurface>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var surface in SystemAPI.Query<RefRO<NavMeshSurface>>())
            {
                ref var vertices = ref surface.ValueRO.data.Value.vertices;
                ref var triangles = ref surface.ValueRO.data.Value.triangles;

                for (int i = 0, size = triangles.Length; i < size; i+=3)
                {
                    var startPoint = vertices[triangles[i]];
                    var middlePoint = vertices[triangles[i + 1]];
                    var endPoint = vertices[triangles[i + 2]];
                    
                    Debug.DrawLine(startPoint, middlePoint);
                    Debug.DrawLine(middlePoint, endPoint);
                    Debug.DrawLine(endPoint, startPoint);
                }
            }
        }
    }
}