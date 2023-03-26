using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
namespace Modules.EnemySpaceship.Runtime.Scripts
{
    /// <summary>
    /// Responsible for creating enemy spaceship whenever it is destroyed
    /// </summary>
    public class EnemySpaceshipCreatorSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            // If there is no enemy spaceship in the scene, create one
            if (!HasSingleton<EnemySpaceshipTag>())
            {
                CreateEnemySpaceship();
            }
        }
        private void CreateEnemySpaceship()
        {
            // Create a random instance
            Random random = new Random((uint)Time.ElapsedTime + 1);

            // Get the prefab container and game settings
            GamePrefabsSingleton prefabContainer = GetSingleton<GamePrefabsSingleton>();
            GameSettingsSingleton gameSettings = GetSingleton<GameSettingsSingleton>();

            // Instantiate the enemy spaceship and add the movement component
            Entity enemySpaceshipEntity = EntityManager.Instantiate(prefabContainer.spaceshipEnemyPrefab);
            MovementComponent movementComponent = RandomMovementComponent.Create(random, gameSettings.enemySpaceshipSpeedRange, gameSettings.enemySpaceshipSpawnRange, gameSettings.enemySpaceshipDirectionRange);
            EntityManager.AddComponentData(enemySpaceshipEntity, movementComponent);
        }
    }
}
