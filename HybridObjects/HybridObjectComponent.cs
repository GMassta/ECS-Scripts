using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EcsScripts.HybridObjects
{
    /// <summary>
    /// Adds a <see cref="HybridObjectPrefab"/> component to the entity.
    /// Instantiates a GameObject from the Addressables system and attaches its MonoBehaviour components to the entity.
    /// </summary>
    /// <param name="PrefabUid">The prefab UID from an <see cref="AssetReference"/>.</param>
    /// <param name="HideInstance">If true, the created instance will be hidden in the hierarchy.</param>
    /// <param name="SetPosition">If true, the entity will be initialized with the specified <paramref name="Position"/> and <paramref name="Rotation"/>.</param>
    /// <param name="Position">The position to apply when initializing the entity.</param>
    /// <param name="Rotation">The rotation to apply when initializing the entity.</param>
    /// </summary>
    
    public class HybridObjectPrefab: IComponentData
    {
        public AsyncOperationHandle LoadHandle;
        public string PrefabUid;

        public bool HideInstance;
        public bool SetPosition;
        public float3 Position = float3.zero;
        public quaternion Rotation = quaternion.identity;
    }
    
    public class HybridObjectInstance : IComponentData, IDisposable
    {
        public GameObject instance;
        public void Dispose() => UnityEngine.Object.DestroyImmediate(instance);
    }
}