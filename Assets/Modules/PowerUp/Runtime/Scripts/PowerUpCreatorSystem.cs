using System;
using Modules.Common.Scripts;
using Modules.Movement.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
namespace Modules.PowerUp.Runtime.Scripts
{
    /// <summary>
    /// Responsible for creating power ups if there is none in the scene.
    /// </summary>
    public class PowerUpCreatorSystem : ComponentSystem
    {
        Random random;
        protected override void OnCreate()
        {
            random = new Random(987954654);
        }
        protected override void OnUpdate()
        {
            // Create a power up if there is none in the scene
            if (!HasSingleton<PowerUpTag>())
            {
                CreateRandomPowerUp();
            }
        }
        private void CreateRandomPowerUp()
        {
            // Get the prefabs and game settings
            GamePrefabsSingleton prefabContainer = GetSingleton<GamePrefabsSingleton>();
            GameSettingsSingleton gameSettings = GetSingleton<GameSettingsSingleton>();

            // Create a random power up
            PowerUpType powerUpType = (PowerUpType)random.NextInt(1, Enum.GetNames(typeof(PowerUpType)).Length);
            Entity powerUpPrefab = powerUpType switch
            {
                PowerUpType.BulletSpeed => prefabContainer.powerUpBulletSpeedPrefab,
                PowerUpType.TripleShot => prefabContainer.powerUpTripleShotPrefab,
                PowerUpType.Shield => prefabContainer.powerUpShieldPrefab,
                _ => Entity.Null
            };

            // Create the power up
            Entity newPowerUpEntity = EntityManager.Instantiate(powerUpPrefab);

            // Set the position and movement component
            MovementComponent movementComponent = RandomMovementComponent.Create(random, float2.zero, gameSettings.powerUpsSpawnRange, float2.zero);
            EntityManager.AddComponentData(newPowerUpEntity, movementComponent);
            EntityManager.SetComponentData(newPowerUpEntity, new PowerUpTag() { powerUpType = powerUpType });
        }
    }
}
