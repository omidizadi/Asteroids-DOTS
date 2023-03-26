using System;
using System.Reflection;
using DefaultNamespace.Configs;
using DefaultNamespace.Entities;
using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace DefaultNamespace.Components
{
    /// <summary>
    /// Represents a shooter component that can shoot a bullet in the game.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ShooterComponent : IComponentData
    {
        //logic
        private float3 position;
        private quaternion rotation;
        private float timeSinceLastShot;

        /// <summary>
        /// Updates the position of the shooter.
        /// </summary>
        /// <param name="position">The new position of the shooter.</param>
        public void SetPosition(float3 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Updates the rotation of the shooter.
        /// </summary>
        /// <param name="rotation">The new rotation of the shooter.</param>
        public void SetRotation(quaternion rotation)
        {
            this.rotation = rotation;
        }

        /// <summary>
        /// Shoots a bullet.
        /// </summary>
        /// <param name="entityManager">The EntityManager that manages the entities in the game.</param>
        /// <param name="config">The configuration settings for the shooter.</param>
        /// <param name="deltaTime">The delta time in the game.</param>
        /// <param name="fireInput">Whether the fire button is pressed or not. Only used for manual shooting</param>
        public void Shoot(EntityManager entityManager, ShooterConfig config, float deltaTime, bool fireInput)
        {
            // If the shooter is in manual fire mode, check if the fire button is pressed
            if (config.autoFireMode == AutoFireMode.Manual)
            {
                if (fireInput)
                {
                    // Shoot a bullet
                    Shoot(entityManager, config);
                }
            }
            // If the shooter is in auto fire mode, shoot automatically according to time between shots
            else
            {
                ShootAutoFire(entityManager, config, deltaTime);
            }
        }

        private void ShootAutoFire(EntityManager entityManager, ShooterConfig config, float deltaTime)
        {
            if (timeSinceLastShot >= config.timeBetweenShots)
            {
                // Shoot a bullet
                Shoot(entityManager, config);
                timeSinceLastShot = 0;
            }
            else
            {
                // Update the time since the last shot
                timeSinceLastShot += deltaTime;
            }
        }

        private void Shoot(EntityManager entityManager, ShooterConfig config)
        {
            // Check for null references
            SanityCheck.NullCheck(config.bulletEntityPrefab, nameof(config.bulletEntityPrefab), nameof(ShooterComponent));

            for (int i = 0; i < config.bulletsCount; i++)
            {
                // Instantiate a bullet entity
                Entity bullet = entityManager.Instantiate(config.bulletEntityPrefab);

                // Calculate the angle of the bullet
                float angle = (config.bulletsCount - 1) * 10 / 2f - i * 10;

                // Create movement config for the bullet
                MovementConfig movementConfig = new MovementConfig()
                {
                    friction = 0,
                    speed = config.bulletSpeed,
                    acceleration = 0
                };

                // Set the bullet's movement properties
                MovementComponent movementComponent = entityManager.GetComponentData<MovementComponent>(bullet);
                float3 direction = math.mul(quaternion.AxisAngle(math.forward(), math.radians(angle)), math.forward(rotation));
                movementComponent.SetConfig(movementConfig);
                movementComponent.SetPosition(position);
                movementComponent.SetDirection(direction);
                entityManager.SetComponentData(bullet, movementComponent);
            }
        }
    }
}
