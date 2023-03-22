using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DefaultNamespace
{
    public class AsteroidsSystem : ComponentSystem
    {
        private NativeArray<AsteroidEntity> asteroidsData;
        private Random random;
        private const int InitialAsteroidsCount = 100;
        bool asteroidsInstantiated = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            asteroidsData = new NativeArray<AsteroidEntity>(InitialAsteroidsCount, Allocator.Persistent);
            random = new Random(8979546);
        }

        protected override void OnUpdate()
        {
            if (!asteroidsInstantiated)
            {
                Entity gamePrefabsContainerEntity = GetSingletonEntity<GamePrefabsContainerEntity>();
                Entity asteroidPrefab = EntityManager.GetComponentData<GamePrefabsContainerEntity>(gamePrefabsContainerEntity).asteroidPrefab;

                for (int i = 0; i < InitialAsteroidsCount; i++)
                {
                    Entity asteroid = EntityManager.Instantiate(asteroidPrefab);
                    AsteroidEntity asteroidEntity = EntityManager.GetComponentData<AsteroidEntity>(asteroid);

                    asteroidEntity.asteroidIndex = i;

                    float3 randomDirection = math.normalize(new float3(random.NextFloat(-1f, 1f), random.NextFloat(-1f, 1f), 0f));
                    asteroidEntity.direction = randomDirection;

                    float randomSpeed = random.NextFloat(10f, 20f);
                    asteroidEntity.speed = randomSpeed;

                    float3 randomPosition = new float3(random.NextFloat(150f, 500f), random.NextFloat(150f, 500f), 0f);
                    asteroidEntity.position = randomPosition;

                    if (random.NextInt(0, 2) == 0)
                    {
                        asteroidEntity.position.x *= -1;
                    }

                    if (random.NextInt(0, 2) == 0)
                    {
                        asteroidEntity.position.y *= -1;
                    }

                    float randomScale = random.NextFloat(500f, 650f);
                    asteroidEntity.scale = randomScale;

                    asteroidEntity.rotation = quaternion.identity;

                    asteroidsData[i] = asteroidEntity;
                    EntityManager.SetComponentData(asteroid, asteroidEntity);
                }

                asteroidsInstantiated = true;
            }

            AsteroidMoveJob moveJob = new AsteroidMoveJob
            {
                deltaTime = Time.DeltaTime,
                asteroidsData = asteroidsData
            };

            JobHandle jobHandle = moveJob.ScheduleParallel(asteroidsData.Length, 64, default(JobHandle));

            jobHandle.Complete();
            if (jobHandle.IsCompleted)
            {
                Entities
                    .WithAll<AsteroidEntity, Translation, Rotation, NonUniformScale>()
                    .ForEach((Entity entity, ref Translation translation, ref Rotation rotation, ref NonUniformScale scale) =>
                    {
                        AsteroidEntity asteroidEntity = EntityManager.GetComponentData<AsteroidEntity>(entity);
                        int entityIndex = asteroidEntity.asteroidIndex;
                        translation.Value = asteroidsData[entityIndex].position;
                        rotation.Value = asteroidsData[entityIndex].rotation;
                        scale.Value = asteroidsData[entityIndex].scale;
                        EntityManager.SetComponentData(entity, asteroidsData[entityIndex]);

                        //TODO: it is O(n^2) complexity, it should be O(nlogn) using KdTree
                        Entities
                            .WithAll<BulletEntity, Translation>()
                            .ForEach((Entity bulletEntity, ref Translation bulletTranslation) =>
                            {
                                float3 bulletPosition = bulletTranslation.Value;
                                if (math.length(bulletPosition - asteroidEntity.position) < 10f)
                                {
                                    PostUpdateCommands.DestroyEntity(bulletEntity);

                                    AsteroidEntity newAsteroidEntity = asteroidEntity;
                                    if (newAsteroidEntity.scale < 500f)
                                    {
                                        PostUpdateCommands.DestroyEntity(entity);
                                        return;
                                    }
                                    //TODO: create two smaller asteroids
                                    newAsteroidEntity.scale *= 0.5f;
                                    newAsteroidEntity.speed *= 1.5f;
                                    newAsteroidEntity.rotation = quaternion.AxisAngle(new float3(0, 0, 1), random.NextFloat(0f, 360f));
                                    asteroidsData[entityIndex] = newAsteroidEntity;
                                }
                            });
                    });
            }
        }


        protected override void OnDestroy()
        {
            asteroidsData.Dispose();
        }
    }
}
