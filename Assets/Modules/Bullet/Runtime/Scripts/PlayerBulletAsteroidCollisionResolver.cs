using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Modules.Rotator.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace DefaultNamespace.Entities
{
    public static class PlayerBulletAsteroidCollisionResolver
    {
        public static void ResolveCollision(EntityManager entityManager, GameSettingsSingleton gameSettingsSingleton, Entity bulletEntity, Entity asteroidEntity)
        {
            //destroy the bulletEntity
            entityManager.DestroyEntity(bulletEntity);


            NonUniformScale asteroidScale = entityManager.GetComponentData<NonUniformScale>(asteroidEntity);
            MovementComponent asteroidMovement = entityManager.GetComponentData<MovementComponent>(asteroidEntity);

            if (asteroidScale.Value.x > gameSettingsSingleton.asteroidMinScale && asteroidScale.Value.y > gameSettingsSingleton.asteroidMinScale)
            {
                CreateSmallerAsteroids(entityManager, asteroidEntity, asteroidScale, asteroidMovement);
            }

            //destroy the asteroidEntity and instantiate 2 smaller asteroids
            entityManager.DestroyEntity(asteroidEntity);
        }

        private static void CreateSmallerAsteroids(EntityManager entityManager, Entity asteroidEntity, NonUniformScale asteroidScale, MovementComponent asteroidMovement)
        {
            for (int i = 0; i < 2; i++)
            {
                Entity newAsteroid = entityManager.Instantiate(asteroidEntity);

                entityManager.SetComponentData(newAsteroid, new NonUniformScale { Value = asteroidScale.Value * 0.5f });

                // Set the collision radius of the new asteroid
                CollisionComponent collision = entityManager.GetComponentData<CollisionComponent>(newAsteroid);
                collision.Radius *= 0.5f;
                entityManager.SetComponentData(newAsteroid, collision);

                // Calculate the angle of the bullet
                float angle = 30 / 2f - i * 30;
                float3 direction = math.mul(quaternion.AxisAngle(math.forward(), math.radians(angle)), asteroidMovement.Direction);
                asteroidMovement.UpdateDirection(direction);
                entityManager.SetComponentData(newAsteroid, asteroidMovement);
            }
        }
    }
}
