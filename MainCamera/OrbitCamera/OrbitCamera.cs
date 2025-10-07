using Unity.Entities;
using Unity.Mathematics;

public struct OrbitCamera : IComponentData
{
    public float RotationSpeed;

    public float StartDistance;
    public float MinDistance;
    public float MaxDistance;
    public float CurrentDistance;

    public float TargetTransitionTime;
    public float StartTransitionTime;
    public Entity CurrentTarget;
    public Entity NextTarget;

    public float3 PlanarForward;
}

public struct OrbitCameraControl : IComponentData
{
    public Entity target;
    public float2 lookDegreesDelta;
    public float zoomDelta;
    public float changeDuration;
}