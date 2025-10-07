using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct CameraControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<OrbitCamera, OrbitCameraControl>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var delta = SystemAPI.Time.DeltaTime;
        foreach (var control in SystemAPI.Query<RefRW<OrbitCameraControl>>())
        {
            if (Input.GetMouseButton(0))
            {
                var mouseDelta = math.clamp(Input.mousePositionDelta, -1, 1);
                control.ValueRW.lookDegreesDelta = new float2(mouseDelta.x, mouseDelta.y);
            }
            else
            {
                if (math.length(control.ValueRO.lookDegreesDelta) > 0)
                    control.ValueRW.lookDegreesDelta = math.lerp(control.ValueRO.lookDegreesDelta, Vector2.zero, 
                        delta);
            }

                
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}