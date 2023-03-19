using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DefaultNamespace
{
    public class AsteroidsSystem : ComponentSystem
    {
        private NativeArray<AsteroidEntity> asteroidsData;
        private Random random;
        private const int asteroidsCount = 50;
        bool asteroidsInstantiated = false;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            asteroidsData = new NativeArray<AsteroidEntity>(asteroidsCount, Allocator.Persistent);
            random = new Random(8979546);
        }

        protected override void OnUpdate()
        {
            if (!asteroidsInstantiated)
            {
                Entity gamePrefabsContainerEntity = GetSingletonEntity<GamePrefabsContainerEntity>();
                Entity asteroidPrefab = EntityManager.GetComponentData<GamePrefabsContainerEntity>(gamePrefabsContainerEntity).asteroidPrefab;

                for (int i = 0; i < asteroidsCount; i++)
                {
                    float3 randomDirection = math.normalize(new float3(random.NextFloat(-1f, 1f), random.NextFloat(-1f, 1f), 0f));
                    float randomSpeed = random.NextFloat(10f, 20f);

                    Entity asteroid = EntityManager.Instantiate(asteroidPrefab);

                    AsteroidEntity asteroidEntity = EntityManager.GetComponentData<AsteroidEntity>(asteroid);
                    asteroidEntity.direction = randomDirection;
                    asteroidEntity.speed = randomSpeed;
                    asteroidEntity.asteroidIndex = i;

                    asteroidEntity.position = new float3(random.NextFloat(150f, 500f), random.NextFloat(150f, 500f), 0f);

                    if (random.NextInt(0, 2) == 0)
                    {
                        asteroidEntity.position.x *= -1;
                    }
                    
                    if (random.NextInt(0, 2) == 0)
                    {
                        asteroidEntity.position.y *= -1;
                    }

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

            JobHandle jobHandleDep = new JobHandle();
            JobHandle jobHandle = moveJob.ScheduleParallel(asteroidsCount, 64, jobHandleDep);

            jobHandle.Complete();

            if (jobHandle.IsCompleted)
            {
                Entities
                    .WithAll<AsteroidEntity, Translation, Rotation>()
                    .ForEach((Entity entity, ref Translation asteroidTranslation, ref Rotation rotation) =>
                    {
                        int entityIndex = EntityManager.GetComponentData<AsteroidEntity>(entity).asteroidIndex;
                        asteroidTranslation.Value = asteroidsData[entityIndex].position;
                        rotation.Value = asteroidsData[entityIndex].rotation;
                    });
            }
        }

        protected override void OnDestroy()
        {
            asteroidsData.Dispose();
        }
    }
}
