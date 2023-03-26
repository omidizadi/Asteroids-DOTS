using Modules.Common.Scripts;
using Modules.Movement.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
namespace Modules.Shooter.Runtime.Scripts
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

            // Check if the fire button is pressed
            if (fireInput)
            {
                Shoot(entityManager, config);
            }
        }

        /// <summary>
        /// Shoots a bullet automatically based on the time between shots.
        /// </summary>
        /// <param name="entityManager">The EntityManager that manages the entities in the game.</param>
        /// <param name="config">The configuration settings for the shooter.</param>
        /// <param name="shooterPosition">The position of the shooter</param>
        /// <param name="shooterRotation">The rotation of the shooter</param>
        /// <param name="deltaTime">The time since the last frame.</param>
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
                const int angleOffset = 10;
                float angle = (config.bulletsCount - 1) * angleOffset / 2f - i * angleOffset;

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
