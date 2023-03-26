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
        private const int InitialAsteroidsCount = 100;
        bool asteroidsInstantiated = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            asteroidsData = new NativeArray<AsteroidEntity>(InitialAsteroidsCount, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (!asteroidsInstantiated)
            {
                asteroidsInstantiated = true;
            }


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
                        .WithAll<OldBulletEntity, Translation>()
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
                                // newAsteroidEntity.rotation = quaternion.AxisAngle(new float3(0, 0, 1), random.NextFloat(0f, 360f));
                                asteroidsData[entityIndex] = newAsteroidEntity;
                            }
                        });
                });
        }


        protected override void OnDestroy()
        {
            asteroidsData.Dispose();
        }
    }
}
