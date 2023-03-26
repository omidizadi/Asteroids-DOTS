using DefaultNamespace.Entities;
using Modules.Asteroids.Runtime.Scripts;
using Modules.PowerUp.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;

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
        return entityManager.HasComponent<EnemySpaceshipTag>(other) || entityManager.HasComponent<AsteroidTag>(other);
    }

    private bool CanCollideWithPlayerSpaceship(Entity other)
    {
        return entityManager.HasComponent<EnemySpaceshipTag>(other) || entityManager.HasComponent<EnemyBulletTag>(other) || entityManager.HasComponent<AsteroidTag>(other) || entityManager.HasComponent<PowerUpTag>(other);
    }
}
