using _Content.Scripts.ECS.EntityAnimator;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EcsScripts.HybridObjects
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct FollowToEntitySystem: ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<HybridObjectInstance, FollowToEntity>()
                .Build());
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, follow, entity) in SystemAPI
                         .Query<RefRO<LocalToWorld>, RefRO<FollowToEntity>>().WithEntityAccess())
            {
                var objTransform = state.EntityManager.GetComponentObject<Transform>(entity);
                if(objTransform == null) continue;

                if (!follow.ValueRO.ignorePosition)
                    objTransform.position = transform.ValueRO.Position;

                if(!follow.ValueRO.ignoreRotation)
                    objTransform.rotation = transform.ValueRO.Rotation;
            }
        }
    }
}