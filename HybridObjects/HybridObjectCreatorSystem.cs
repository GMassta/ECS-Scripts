using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EcsScripts.HybridObjects
{
    public partial class HybridObjectCreatorSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<HybridObjectPrefab>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in HybridObjectPrefab prefab) =>
            {
                if (!prefab.LoadHandle.IsValid())
                {
                    prefab.LoadHandle = Addressables
                        .InstantiateAsync(prefab.PrefabUid, prefab.Position, prefab.Rotation);
                }
                else if(prefab.LoadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var instance = prefab.LoadHandle.Result as GameObject;
                    var transform = instance.GetComponent<Transform>();
                    instance.name = instance.name.Replace("(Clone)", $"[{entity.Index}]");
                    
                    if (prefab.HideInstance) 
                        instance.hideFlags |= HideFlags.HideAndDontSave;

                    if (prefab.SetPosition)
                    {
                        EntityManager.AddComponentData(entity, LocalTransform
                            .FromPositionRotation(prefab.Position, prefab.Rotation));
                    }

                    EntityManager.AddComponentObject(entity, transform);
                    AddComponents(EntityManager, entity, instance);
                    
                    EntityManager.AddComponentData(entity, new HybridObjectInstance { instance = instance });
                    EntityManager.RemoveComponent<HybridObjectPrefab>(entity);
                }
            }).WithoutBurst().WithStructuralChanges().Run();
        }
        
        private void AddComponents(EntityManager manager, Entity entity, GameObject instance)
        {
            var components = instance.GetComponents<Component>();
            foreach (var component in components)
                manager.AddComponentObject(entity, component);
        }
    }
}