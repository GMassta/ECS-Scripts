using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsScripts.SimpleNavMesh
{
    public partial struct SimpleNavMeshSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CheckPointOnSurface>();
            state.RequireForUpdate<NavMeshSurface>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var surface = SystemAPI.GetSingleton<NavMeshSurface>();
            var job = new CalculatePointJob
            {
                surface = surface
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct CalculatePointJob: IJobEntity
    {
        public NavMeshSurface surface;
        
        [BurstCompile]
        private void Execute(Entity entity, ref CheckPointOnSurface checkPoint)
        {
            if(!surface.data.IsCreated)
                 return;

            ref var vertices = ref surface.data.Value.vertices;
            ref var triangles = ref surface.data.Value.triangles;
            var minDistance = math.INFINITY;

            for (int i = 0, size = triangles.Length; i < size; i+=3)
            {
                var v1 = vertices[triangles[i]];
                var v2 = vertices[triangles[i + 1]];
                var v3 = vertices[triangles[i + 2]];

                var point = checkPoint.checkPoint;
                point.y = CalculatePointY(point, v1, v2, v3);
                if (IsPointInTriangle(point, v1, v2, v3))
                {
                    checkPoint.foundPoint = point;
                    checkPoint.onBoard = false;
                    return;
                }

                var closest = ClosestPointOnTriangle(point, v1, v2, v3);
                var distance = math.distance(point, closest);

                if (!(distance < minDistance)) continue;

                checkPoint.foundPoint = closest;
                checkPoint.onBoard = true;
                minDistance = distance;
            }
        }

        private float CalculatePointY(float3 p, float3 a, float3 b, float3 c)
        {
            var e1 = b - a;
            var e2 = c - a;
            
            var normal = math.cross(e1, e2);
            normal = math.normalize(normal);

            var d = math.dot(p - a, normal);
            return (p - d * normal).y;
        }

        private float3 ClosestPointOnTriangle(float3 p, float3 a, float3 b, float3 c)
        {
            var closestAB = ClosestPointOnLine(a, b, p);
            var closestBC = ClosestPointOnLine(b, c, p);
            var closestCA = ClosestPointOnLine(c, a, p);

            var distAB = math.distance(p, closestAB);
            var distBC = math.distance(p, closestBC);
            var distCA = math.distance(p, closestCA);

            var minDist = math.min(math.min(distAB, distBC), distCA);
            if (Mathf.Approximately(minDist, distAB)) return closestAB;
            if (Mathf.Approximately(minDist, distBC)) return closestBC;
            return closestCA;
        }

        private float3 ClosestPointOnLine(float3 a, float3 b, float3 p)
        {
            var ab = b - a;
            var t = math.dot(p - a, ab) / math.dot(ab, ab);
            t = math.clamp(t, 0, 1);
            return a + t * ab;
        }

        private bool IsPointInTriangle(float3 p, float3 a, float3 b, float3 c)
        {
            var v0 = c - a;
            var v1 = b - a;
            var v2 = p - a;
            
            var dot00 = math.dot(v0, v0);
            var dot01 = math.dot(v0, v1);
            var dot02 = math.dot(v0, v2);
            var dot11 = math.dot(v1, v1);
            var dot12 = math.dot(v1, v2);
            
            var invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            
            return (u >= 0f) && (v >= 0f) && (u + v <= 1f);
        }
    }
}