using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Modules.PowerUp.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DefaultNamespace
{
    public class PowerUpCreatorSystem : ComponentSystem
    {
        private Random random;

        protected override void OnCreate()
        {
            base.OnCreate();
            random = new Random(8979546);
        }
        protected override void OnUpdate()
        {
            if (!HasSingleton<PowerUpTag>())
            {
                CreateRandomPowerUp();
            }
        }
        private void CreateRandomPowerUp()
        {
            GamePrefabsSingleton prefabContainer = GetSingleton<GamePrefabsSingleton>();
            GameSettingsSingleton gameSettings = GetSingleton<GameSettingsSingleton>();
            PowerUpType powerUpType = (PowerUpType)random.NextInt(1, 4);
            Entity powerUpPrefab = powerUpType switch
            {
                PowerUpType.BulletSpeed => prefabContainer.powerUpBulletSpeedPrefab,
                PowerUpType.TripleShot => prefabContainer.powerUpTripleShotPrefab,
                PowerUpType.Shield => prefabContainer.powerUpShieldPrefab,
                _ => Entity.Null
            };
            Entity newPowerUpEntity = EntityManager.Instantiate(powerUpPrefab);
            MovementComponent movementComponent = RandomMovementComponent.Create(random, float2.zero, gameSettings.powerUpsSpawnRange, float2.zero);
            EntityManager.AddComponentData(newPowerUpEntity, movementComponent);
            EntityManager.SetComponentData(newPowerUpEntity, new PowerUpTag() { powerUpType = powerUpType });
        }
    }
}
