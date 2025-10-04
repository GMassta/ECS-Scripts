using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class MainCameraAuthoring : MonoBehaviour
{
    private class MainCameraAuthoringBaker : Baker<MainCameraAuthoring>
    {
        public override void Bake(MainCameraAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<MainEntityCamera>(entity);
        }
    }
}