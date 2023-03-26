using DefaultNamespace.Entities;
using Modules.Asteroids.Runtime.Scripts;
using Modules.Common.Scripts;
using Modules.PowerUp.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using UnityEngine;
namespace Modules.Collision.Runtime.Scripts
{
    [UpdateAfter(typeof(CollisionDetectionSystem))]
    public class CollisionResolveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CollisionComponent>()
                .ForEach((Entity entity, ref CollisionComponent collisionComponent) =>
                {
                    if (collisionComponent.CollidedEntity != Entity.Null)
                    {
                        if (EntityManager.HasComponent<PlayerSpaceshipTag>(entity))
                        {
                            HandlePlayerSpaceshipCollision(collisionComponent, entity);
                        }
                        if (EntityManager.HasComponent<PlayerBulletTag>(entity))
                        {
                            HandlePlayerBulletCollision(collisionComponent, entity);
                        }
                    }
                });
        }
        
        private void HandlePlayerBulletCollision(CollisionComponent collisionComponent, Entity entity)
        {
            if (EntityManager.HasComponent<AsteroidTag>(collisionComponent.CollidedEntity))
            {
                PlayerBulletAsteroidCollisionResolver.ResolveCollision(EntityManager, GetSingleton<GameSettingsSingleton>(), entity, collisionComponent.CollidedEntity);
            }
            if (EntityManager.HasComponent<EnemySpaceshipTag>(collisionComponent.CollidedEntity))
            {
                EntityManager.DestroyEntity(entity);
                EntityManager.DestroyEntity(collisionComponent.CollidedEntity);
            }
        }
        
        private void HandlePlayerSpaceshipCollision(CollisionComponent collisionComponent, Entity entity)
        {
            if (EntityManager.HasComponent<AsteroidTag>(collisionComponent.CollidedEntity) ||
                EntityManager.HasComponent<EnemySpaceshipTag>(collisionComponent.CollidedEntity) ||
                EntityManager.HasComponent<EnemyBulletTag>(collisionComponent.CollidedEntity))
            {
                EntityManager.DestroyEntity(entity);
            }
            if (EntityManager.HasComponent<PowerUpTag>(collisionComponent.CollidedEntity))
            {
                PlayerSpaceshipPowerUpCollisionResolver.Resolve(EntityManager, entity, collisionComponent.CollidedEntity);
            }
        }
    }
}
