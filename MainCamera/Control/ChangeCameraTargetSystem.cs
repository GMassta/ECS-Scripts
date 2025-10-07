using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace _Content.Scripts.ECS.Camera.Control
{
    public partial struct ChangeCameraTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ChangeCameraTarget>();
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<OrbitCameraControl, OrbitCamera>().Build());
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var cameraControl = SystemAPI.GetSingletonRW<OrbitCameraControl>();
            foreach (var (cfg, entity) in SystemAPI.Query<RefRO<ChangeCameraTarget>>().WithEntityAccess())
            {
                cameraControl.ValueRW.target = entity;
                cameraControl.ValueRW.changeDuration = cfg.ValueRO.Duration;
                ecb.RemoveComponent<ChangeCameraTarget>(entity);
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}