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
        /// Shoots a bullet.
        /// </summary>
        /// <param name="entityManager">The EntityManager that manages the entities in the game.</param>
        /// <param name="config">The configuration settings for the shooter.</param>
        /// <param name="shooterPosition">The position of the shooter</param>
        /// <param name="shooterRotation">The rotation of the shooter</param>
        /// <param name="fireInput">Whether the fire button is pressed or not. Only used for manual shooting</param>
        public void ShootManual(EntityManager entityManager, ShooterConfig config, float3 shooterPosition, quaternion shooterRotation, bool fireInput)
        {
            // Update the initial position and rotation of the shooter
            position = shooterPosition;
            rotation = shooterRotation;

            // If the shooter is in manual fire mode, check if the fire button is pressed
            if (config.autoFireMode == AutoFireMode.Manual)
            {
                if (fireInput)
                {
                    // Shoot a bullet
                    Shoot(entityManager, config);
                }
            }
        }

        public void ShootAuto(EntityManager entityManager, ShooterConfig config, float3 shooterPosition, quaternion shooterRotation, float deltaTime)
        {
            // Update the initial position and rotation of the shooter
            position = shooterPosition;
            rotation = shooterRotation;

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

                // Calculate the direction of the bullet
                float3 direction = math.mul(quaternion.AxisAngle(math.forward(), math.radians(angle)), math.forward(rotation));

                // Create movement config for the bullet
                MovementConfig movementConfig = new MovementConfig(config.bulletSpeed, 0f, 0f);

                // Set the bullet's movement properties
                MovementComponent movementComponent = new MovementComponent(movementConfig, position, direction);
                entityManager.SetComponentData(bullet, movementComponent);
            }
        }
    }
}
