using DefaultNamespace;
using DefaultNamespace.Configs;
using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Modules.PowerUp.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
namespace Modules.EnemySpaceship.Runtime.Scripts
{
    public class EnemySpaceshipCreatorSystem : ComponentSystem
    {
        private Random random;

        protected override void OnCreate()
        {
            base.OnCreate();
            random = new Random(8979546);
        }
        protected override void OnUpdate()
        {
            if (!HasSingleton<EnemySpaceshipTag>())
            {
                CreateEnemySpaceship();
            }
        }
        private void CreateEnemySpaceship()
        {
            GamePrefabsSingleton prefabContainer = GetSingleton<GamePrefabsSingleton>();
            GameSettingsSingleton gameSettings = GetSingleton<GameSettingsSingleton>();

            Entity enemySpaceshipEntity = EntityManager.Instantiate(prefabContainer.spaceshipEnemyPrefab);

            MovementComponent movementComponent = RandomMovementComponent.Create(random, gameSettings.enemySpaceshipSpeedRange, gameSettings.enemySpaceshipSpawnRange, gameSettings.enemySpaceshipDirectionRange);
            EntityManager.AddComponentData(enemySpaceshipEntity, movementComponent);
        }
    }
}
