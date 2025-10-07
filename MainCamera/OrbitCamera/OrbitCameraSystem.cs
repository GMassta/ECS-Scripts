using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct OrbitCameraSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<OrbitCamera, OrbitCameraControl>().Build());
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var time = SystemAPI.Time;
        
        var job = new CameraJob
        {
            DeltaTime = time.DeltaTime,
            ElapsedTime = (float) time.ElapsedTime,
            LtWLookup = SystemAPI.GetComponentLookup<LocalToWorld>(),
        };
        job.Schedule();
    }
        
    [BurstCompile]
    [WithAll(typeof(Simulate))]
    public partial struct CameraJob: IJobEntity
    {
        public float DeltaTime;
        public float ElapsedTime;
        public ComponentLookup<LocalToWorld> LtWLookup;

        void Execute(Entity entity, ref LocalTransform transform, ref OrbitCamera camera, in 
            OrbitCameraControl control)
        {
            //Change Target
            if (camera.CurrentTarget != control.target)
            {
                if (camera.NextTarget != control.target)
                {
                    camera.NextTarget = control.target;
                    camera.StartTransitionTime = ElapsedTime;
                }

                if (LtWLookup.TryGetComponent(camera.NextTarget, out var nextTargetTransform))
                {
                    if (ElapsedTime < camera.StartTransitionTime + control.changeDuration)
                    {
                        float3 position = default;

                        if (camera.CurrentTarget != Entity.Null &&
                            LtWLookup.TryGetComponent(camera.CurrentTarget, out var currentTransform))
                        {
                            position = currentTransform.Position;
                        }
                        else
                        {
                            position = transform.Position;
                        }

                        var rate = math.saturate((ElapsedTime - camera.StartTransitionTime) / control.changeDuration);
                        transform.Position = math.lerp(position, nextTargetTransform.Position, rate);
                    }
                    else
                    {
                        transform.Position = nextTargetTransform.Position;
                        camera.CurrentTarget = camera.NextTarget;
                    }
                }
            }
            else if (LtWLookup.TryGetComponent(camera.CurrentTarget, out var nextTargetTransform))
            {
                transform.Position = nextTargetTransform.Position;
            }

            //Rotate by control
            var yaw = control.lookDegreesDelta.x * camera.RotationSpeed;
            var rotation = quaternion.Euler(math.up() * math.radians(yaw));
            camera.PlanarForward = math.rotate(rotation, camera.PlanarForward);

            transform.Rotation = quaternion.LookRotationSafe(camera.PlanarForward, math.up());
            
            //Add Distance
            transform.Position += -camera.PlanarForward * camera.CurrentDistance;
        }
    }
}