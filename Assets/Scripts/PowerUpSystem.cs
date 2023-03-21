using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DefaultNamespace
{
    public class PowerUpSystem : ComponentSystem
    {
        private Random random;

        protected override void OnCreate()
        {
            base.OnCreate();
            random = new Random(8979546);
        }
        protected override void OnUpdate()
        {
            if (!HasSingleton<PowerUpEntity>())
            {
                CreateRandomPowerUp();
                return;
            }
            SpaceshipEntity spaceship = GetSingleton<SpaceshipEntity>();
            PowerUpEntity powerUp = GetSingleton<PowerUpEntity>();
            Entities
                .WithAll<PowerUpEntity, Translation>()
                .ForEach((Entity powerUpEntity, ref Translation translation) =>
                {
                    //get the translation component of the spaceship
                    if (math.length(spaceship.position - translation.Value) < 20f)
                    {
                        PostUpdateCommands.DestroyEntity(powerUpEntity);
                        spaceship.powerUpType = powerUp.powerUpType;
                        SetSingleton(spaceship);
                        CreateRandomPowerUp();
                    }
                });
        }
        private void CreateRandomPowerUp()
        {
            GamePrefabsContainerEntity prefabContainer = GetSingleton<GamePrefabsContainerEntity>();
            PowerUpType powerUpType = (PowerUpType)random.NextInt(1, 3);
            Entity powerUpPrefab = powerUpType == PowerUpType.Shield ? prefabContainer.powerUpShieldPrefab : prefabContainer.powerUpTripleShotPrefab;
            float3 powerUpPosition = new float3(random.NextFloat(-100, 100), random.NextFloat(-100, 100), 0);
            Entity newPowerUpEntity = EntityManager.Instantiate(powerUpPrefab);
            EntityManager.SetComponentData(newPowerUpEntity, new Translation { Value = powerUpPosition });
            EntityManager.SetComponentData(newPowerUpEntity, new PowerUpEntity { powerUpType = powerUpType });
        }
    }
}
