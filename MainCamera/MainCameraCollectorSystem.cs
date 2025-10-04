using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class MainCameraCollectorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (MainCameraInstance.instance != null && SystemAPI.HasSingleton<MainEntityCamera>())
        {
            var cameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            var entityCameraTransform = SystemAPI.GetComponent<LocalTransform>(cameraEntity);
            
            MainCameraInstance.instance.transform
                .SetPositionAndRotation(entityCameraTransform.Position, entityCameraTransform.Rotation);
        }
    }
}