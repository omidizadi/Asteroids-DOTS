using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace DefaultNamespace.Entities
{
    /// <summary>
    /// Provides a method to resolve the collision between a player bullet and an asteroid
    /// </summary>
    public static class PlayerBulletAsteroidCollisionResolver
    {
        public static void ResolveCollision(EntityManager entityManager, GameSettingsSingleton gameSettingsSingleton, Entity bulletEntity, Entity asteroidEntity)
        {
            entityManager.DestroyEntity(bulletEntity);

            // Get the scale and movement of the asteroid
            NonUniformScale asteroidScale = entityManager.GetComponentData<NonUniformScale>(asteroidEntity);
            MovementComponent asteroidMovement = entityManager.GetComponentData<MovementComponent>(asteroidEntity);

            // If the asteroid is big enough, create two smaller asteroids
            if (asteroidScale.Value.x > gameSettingsSingleton.asteroidMinScale && asteroidScale.Value.y > gameSettingsSingleton.asteroidMinScale)
            {
                CreateSmallerAsteroids(entityManager, asteroidEntity, asteroidScale, asteroidMovement);
            }

            entityManager.DestroyEntity(asteroidEntity);
        }

        private static void CreateSmallerAsteroids(EntityManager entityManager, Entity asteroidEntity, NonUniformScale asteroidScale, MovementComponent asteroidMovement)
        {
            for (int i = 0; i < 2; i++)
            {
                Entity newAsteroid = entityManager.Instantiate(asteroidEntity);

                // Set the scale of the new asteroid
                entityManager.SetComponentData(newAsteroid, new NonUniformScale { Value = asteroidScale.Value * 0.5f });

                // Set the collision radius of the new asteroid
                CollisionComponent collision = entityManager.GetComponentData<CollisionComponent>(newAsteroid);
                collision.Radius *= 0.5f;
                entityManager.SetComponentData(newAsteroid, collision);

                // Calculate the angle of the bullet
                const int angleOffset = 30;
                float angle = angleOffset / 2f - i * angleOffset;
                float3 direction = math.mul(quaternion.AxisAngle(math.forward(), math.radians(angle)), asteroidMovement.Direction);
                asteroidMovement.UpdateDirection(direction);
                entityManager.SetComponentData(newAsteroid, asteroidMovement);
            }
        }
    }
}
