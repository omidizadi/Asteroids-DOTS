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
        //config
        private MovementConfig config;

        //properties
        public float3 Position => position;
        public float3 Direction => direction;

        //logic
        private float3 position;
        private float3 direction;
        private float3 velocity;

        public MovementComponent(MovementConfig config, float3 position, float3 direction)
        {
            this.config = config;
            this.position = position;
            this.direction = direction;
            velocity = default(float3);
        }


        public void UpdateConfig(MovementConfig config)
        {
            this.config = config;
        }

        public void UpdatePosition(float3 position)
        {
            this.position = position;
        }

        public void UpdateDirection(float3 direction)
        {
            this.direction = direction;
        }

        public void UpdateDirection(Vector2 direction)
        {
            Debug.Log(direction);
            this.direction = new float3(direction.x, direction.y, 0f);
        }

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
            if (math.length(velocity) >= 0.1f)
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
