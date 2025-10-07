using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraTargetAuthoring : MonoBehaviour
{
    [SerializeField] private float duration = 0f;
    private class CameraTargetAuthoringBaker : Baker<CameraTargetAuthoring>
    {
        public override void Bake(CameraTargetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ChangeCameraTarget
            {
                Duration = authoring.duration,
            });
        }
    }
}

public struct ChangeCameraTarget : IComponentData
{
    public float Duration;
}