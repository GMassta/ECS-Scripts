using Unity.Entities;
using UnityEngine;

namespace EcsScripts.HybridObjects
{
    /// <summary>
    /// The <see cref="FollowToEntity"/> component moves a hybrid object to follow its associated entity.
    /// </summary>
    /// <param name="ignorePosition">If true, position changes will be ignored.</param>
    /// <param name="ignoreRotation">If true, rotation changes will be ignored.</param>
    public struct FollowToEntity : IComponentData
    {
        public bool ignorePosition;
        public bool ignoreRotation;
    }
}