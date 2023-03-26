using System;
using DefaultNamespace;
using DefaultNamespace.Configs;
using Modules.PowerUp.Runtime.Scripts;
using Unity.Entities;
namespace Modules.Spaceship.Runtime.Scripts
{
    /// <summary>
    /// Provides the logic for resolving the collision between the player spaceship and a power up.
    /// </summary>
    public static class PlayerSpaceshipPowerUpCollisionResolver
    {
        public static void Resolve(EntityManager entityManager, Entity playerSpaceshipEntity, Entity powerUpEntity)
        {
            PowerUpTag powerUpTag = entityManager.GetComponentData<PowerUpTag>(powerUpEntity);

            // Just to make sure that the player spaceship is not invincible for eternity
            EnableCollisionComponent(entityManager, playerSpaceshipEntity);

            switch (powerUpTag.powerUpType)
            {
                case PowerUpType.Shield:
                {
                    PerformShieldPowerUp(entityManager, playerSpaceshipEntity);
                    break;
                }
                case PowerUpType.BulletSpeed:
                {
                    PerformBulletSpeedPowerUp(entityManager, playerSpaceshipEntity);
                    break;
                }
                case PowerUpType.TripleShot:
                {
                    PerformTripleShotPowerUp(entityManager, playerSpaceshipEntity);
                    break;
                }
                case PowerUpType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            entityManager.DestroyEntity(powerUpEntity);
        }
        private static void EnableCollisionComponent(EntityManager entityManager, Entity playerSpaceshipEntity)
        {
            if (entityManager.HasComponent<CollisionComponent>(playerSpaceshipEntity))
            {
                CollisionComponent collision = entityManager.GetComponentData<CollisionComponent>(playerSpaceshipEntity);
                collision.Enable();
                entityManager.SetComponentData(playerSpaceshipEntity, collision);
            }
        }

        private static void PerformTripleShotPowerUp(EntityManager entityManager, Entity playerSpaceshipEntity)
        {
            ShooterConfig shooterConfig = entityManager.GetComponentData<ShooterConfig>(playerSpaceshipEntity);
            if (shooterConfig.bulletsCount < 6)
            {
                shooterConfig.bulletsCount++;
                entityManager.SetComponentData(playerSpaceshipEntity, shooterConfig);
            }
        }

        private static void PerformBulletSpeedPowerUp(EntityManager entityManager, Entity playerSpaceshipEntity)
        {
            ShooterConfig shooterConfig = entityManager.GetComponentData<ShooterConfig>(playerSpaceshipEntity);
            if (shooterConfig.bulletSpeed < 500f)
            {
                shooterConfig.bulletSpeed += 50f;
                entityManager.SetComponentData(playerSpaceshipEntity, shooterConfig);
            }
        }

        private static void PerformShieldPowerUp(EntityManager entityManager, Entity playerSpaceshipEntity)
        {
            if (entityManager.HasComponent<CollisionComponent>(playerSpaceshipEntity))
            {
                CollisionComponent collision = entityManager.GetComponentData<CollisionComponent>(playerSpaceshipEntity);
                collision.Disable();
                entityManager.SetComponentData(playerSpaceshipEntity, collision);
            }
        }
    }
}
