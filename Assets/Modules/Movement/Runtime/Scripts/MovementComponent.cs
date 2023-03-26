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
        //config
        private MovementConfig config;

        //properties
        public float3 Position => position;
        public float3 Direction => direction;

        //logic
        private float3 position;
        private float3 direction;
        private float3 velocity;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementComponent"/> struct.
        /// </summary>
        /// <param name="config">The movement config.</param>
        /// <param name="position">The initial position.</param>
        /// <param name="direction">The initial direction.</param>
        public MovementComponent(MovementConfig config, float3 position, float3 direction)
        {
            this.config = config;
            this.position = position;
            this.direction = direction;
            velocity = default(float3);
        }


        /// <summary>
        /// Updates the config of the movement component
        /// </summary>
        /// <param name="config">The new config</param>
        public void UpdateConfig(MovementConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Updates the position of the movement component
        /// </summary>
        /// <param name="position">The new position</param>
        public void UpdatePosition(float3 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Updates the direction of the movement component
        /// </summary>
        /// <param name="direction">The new direction</param>
        public void UpdateDirection(float3 direction)
        {
            this.direction = direction;
        }

        /// <summary>
        /// Updates the direction of the movement component
        /// </summary>
        /// <param name="direction">The new direction</param>
        public void UpdateDirection(Vector2 direction)
        {
            this.direction = new float3(direction.x, direction.y, 0f);
        }

        /// <summary>
        /// Moves the entity based on the current direction and velocity
        /// </summary>
        /// <param name="deltaTime">The delta time that determines the speed of the movement</param>
        public float3 Move(float deltaTime)
        {
            if (math.length(direction) > 0f)
            {
                // Calculate the velocity
                HandleVelocity(deltaTime);
            }
            else
            {
                HandleFriction(deltaTime);
            }
            position += velocity;
            return position;
        }

        private void HandleVelocity(float deltaTime)
        {
            float3 move = new float3(direction.x, direction.y, 0f);
            float3 normalizedDirection = math.normalize(move);
            velocity = velocity * config.acceleration + normalizedDirection * config.speed * deltaTime;
        }

        private void HandleFriction(float deltaTime)
        {
            //if the velocity is not zero, apply friction
            const float epsilon = 0.0001f;
            if (math.length(velocity) >= epsilon)
            {
                velocity -= math.normalize(velocity) * config.friction * deltaTime;
            }
            else
            {
                velocity = new float3(0f, 0f, 0f);
            }
        }
    }
}
