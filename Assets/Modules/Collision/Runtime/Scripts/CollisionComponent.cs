using Modules.Asteroids.Runtime.Scripts;
using Modules.Bullet.Runtime.Scripts;
using Modules.PlayerSpaceship.Runtime.Scripts;
using Modules.PowerUp.Runtime.Scripts;
using Unity.Entities;
using UnityEngine;
namespace Modules.Collision.Runtime.Scripts
{
    /// <summary>
    /// If the entity has this component, it can collide with other entities
    /// </summary>
    [GenerateAuthoringComponent]
    public struct CollisionComponent : IComponentData
    {
        //config
        public float Radius;

        //dependencies
        private EntityManager entityManager;

        //logic
        private Entity collidedEntity;
        public Entity CollidedEntity => collidedEntity;
        private bool disabled;

        /// <summary>
        /// Disables the collision component so that it will not collide with other entities except the power ups
        /// </summary>
        public void Disable()
        {
            disabled = true;
        }

        /// <summary>
        /// Enables the collision component so that it will collide with other entities
        /// </summary>
        public void Enable()
        {
            disabled = false;
        }

        /// <summary>
        /// Updates the collided entity if the collision is valid
        /// </summary>
        public void UpdateCollisionStatus(EntityManager entityManager, Entity myEntity, Entity other)
        {
            this.entityManager = entityManager;
            if (CanCollideWith(myEntity, other))
            {
                collidedEntity = other;
            }
        }

        private bool CanCollideWith(Entity myEntity, Entity other)
        {
            if (entityManager.HasComponent<PlayerSpaceshipTag>(myEntity))
            {
                return CanCollideWithPlayerSpaceship(other);
            }

            if (entityManager.HasComponent<PlayerBulletTag>(myEntity))
            {
                return CanCollideWithPlayerBullet(other);
            }

            return false;
        }

        private bool CanCollideWithPlayerBullet(Entity other)
        {
            return entityManager.HasComponent<EnemySpaceshipTag>(other) && !disabled
                || entityManager.HasComponent<AsteroidTag>(other) && !disabled;
        }

        private bool CanCollideWithPlayerSpaceship(Entity other)
        {
            Debug.Log($"{entityManager.HasComponent<EnemySpaceshipTag>(other) && !disabled} {entityManager.HasComponent<EnemyBulletTag>(other) && !disabled} {entityManager.HasComponent<AsteroidTag>(other) && !disabled} {entityManager.HasComponent<PowerUpTag>(other)}");
            return entityManager.HasComponent<EnemySpaceshipTag>(other) && !disabled
                || entityManager.HasComponent<EnemyBulletTag>(other) && !disabled
                || entityManager.HasComponent<AsteroidTag>(other) && !disabled
                || entityManager.HasComponent<PowerUpTag>(other);
        }
    }
}
