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
    private bool disabled;

    public void Disable()
    {
        disabled = true;
    }

    public void Enable()
    {
        disabled = false;
    }

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
        return entityManager.HasComponent<EnemySpaceshipTag>(other) && !disabled
            || entityManager.HasComponent<EnemyBulletTag>(other) && !disabled
            || entityManager.HasComponent<AsteroidTag>(other) && !disabled
            || entityManager.HasComponent<PowerUpTag>(other);
    }
}
