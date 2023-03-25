using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace Modules.Mover.Runtime.Scripts
{
    /// <summary>
    /// An entity with this component will be moved by <see cref="MovementSystem"/>
    /// </summary>
    [GenerateAuthoringComponent]
    public struct MovementComponent : IComponentData
    {
        public float3 Position { get; set; }
        public float3 Direction { get; set; }
        public float Speed { get; set; }

        public void Move(float deltaTime)
        {
            Position += Direction * Speed * deltaTime;
        }
    }
}
