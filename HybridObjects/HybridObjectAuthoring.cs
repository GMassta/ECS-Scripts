using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EcsScripts.HybridObjects
{
    /// <summary>
    /// Example of creating a hybrid object authoring
    /// </summary>
    
    public class HybridObjectAuthoring : MonoBehaviour
    {
        [SerializeField] private AssetReference CharacterView;
        private class HybridObjectAuthoringBaker : Baker<HybridObjectAuthoring>
        {
            public override void Bake(HybridObjectAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var hybrid = new HybridObjectPrefab
                {
                    PrefabUid = authoring.CharacterView.AssetGUID
                };

                AddComponentObject(entity, hybrid);
            }
        }
    }
}