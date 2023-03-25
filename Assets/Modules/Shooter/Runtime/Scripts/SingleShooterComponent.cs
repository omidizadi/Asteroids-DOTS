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
    public struct SingleShooterComponent : IComponentData, IShooterComponent
    {
        /// <summary>
        /// The position of the shooter.
        /// </summary>
        public float3 Position { get; set; }

        /// <summary>
        /// The rotation of the shooter.
        /// </summary>
        public quaternion Rotation { get; set; }

        // Logic for determining when to shoot next
        private float timeSinceLastShot;

        /// <summary>
        /// Updates the position and rotation of the shooter.
        /// </summary>
        /// <param name="position">The new position of the shooter.</param>
        /// <param name="rotation">The new rotation of the shooter.</param>
        public void Update(float3 position, quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
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
            SanityCheck.NullCheck(config.bulletEntityPrefab, nameof(config.bulletEntityPrefab), nameof(SingleShooterComponent));

            // Instantiate a bullet entity
            Entity bullet = entityManager.Instantiate(config.bulletEntityPrefab);

            // Set the bullet's movement properties
            MovementComponent movementComponent = entityManager.GetComponentData<MovementComponent>(bullet);
            movementComponent.Position = Position;
            movementComponent.Direction = math.forward(Rotation);
            movementComponent.Speed = config.bulletSpeed;
            entityManager.SetComponentData(bullet, movementComponent);
        }
    }
}
