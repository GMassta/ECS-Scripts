using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class OrbitCameraAuthoring : MonoBehaviour
{
    [Header("Rotation")] 
    [SerializeField] private Vector3 direction = -Vector3.forward;
    [SerializeField] private float rotationSpeed = 2f;
        
    [Header("Distance")]
    [SerializeField] private float startDistance = 5;
    [SerializeField] private float minDistance = 1;
    [SerializeField] private float maxDistance = 6;

    [Header("Misc")] 
    [SerializeField] private float targetTransitionTime = .6f;
    private class OrbitCameraAuthoringBaker : Baker<OrbitCameraAuthoring>
    {
        public override void Bake(OrbitCameraAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity , new OrbitCamera
            {
                TargetTransitionTime = authoring.targetTransitionTime,
                RotationSpeed = authoring.rotationSpeed,
                StartDistance = authoring.startDistance,
                MinDistance = authoring.minDistance,
                MaxDistance = authoring.maxDistance,
                
                CurrentDistance =  authoring.startDistance,
                    
                PlanarForward = authoring.direction,
            });
            AddComponent(entity, new OrbitCameraControl());
        }
    }
}