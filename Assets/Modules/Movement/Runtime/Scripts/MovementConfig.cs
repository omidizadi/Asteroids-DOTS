using Unity.Entities;
namespace Modules.Movement.Runtime.Scripts
{
    /// <summary>
    /// Holds the configuration for the movement of an entity.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct MovementConfig : IComponentData
    {
        public float speed;
        public float acceleration;
        public float friction;
        
        public MovementConfig(float speed, float acceleration, float friction)
        {
            this.speed = speed;
            this.acceleration = acceleration;
            this.friction = friction;
        }
    }
}
