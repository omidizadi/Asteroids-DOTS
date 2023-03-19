using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
namespace DefaultNamespace
{
    public class AsteroidsSystem : ComponentSystem
    {
        private NativeArray<AsteroidEntity> asteroidsData;
        private Random random;
        private const int InitialAsteroidsCount = 50;
        private const int asteroidsCapacity = 5000;
        bool asteroidsInstantiated = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            asteroidsData = new NativeArray<AsteroidEntity>(asteroidsCapacity, Allocator.Persistent);
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
                        int entityIndex = EntityManager.GetComponentData<AsteroidEntity>(entity).asteroidIndex;
                        translation.Value = asteroidsData[entityIndex].position;
                        rotation.Value = asteroidsData[entityIndex].rotation;
                        scale.Value = asteroidsData[entityIndex].scale;
                        EntityManager.SetComponentData(entity, asteroidsData[entityIndex]);
                    });
            }
        }

        protected override void OnDestroy()
        {
            asteroidsData.Dispose();
        }
    }
}
