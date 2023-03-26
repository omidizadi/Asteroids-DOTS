using DefaultNamespace;
using Modules.Common.Scripts;
using Modules.Mover.Runtime.Scripts;
using Modules.Rotator.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
namespace Modules.Asteroids.Runtime.Scripts
{
    public class AsteroidsCreatorSystem : ComponentSystem
    {
        private bool asteroidsCreated;
        private Random random;

        protected override void OnCreate()
        {
            random = new Random(8979546);
        }

        protected override void OnUpdate()
        {
            if (!asteroidsCreated)
            {
                CreateAsteroids();
                asteroidsCreated = true;
            }
        }
        private void CreateAsteroids()
        {
            // Grab the asteroid prefab from the GamePrefabsSingleton to instantiate
            Entity gamePrefabsContainerEntity = GetSingletonEntity<GamePrefabsSingleton>();
            Entity asteroidPrefab = EntityManager.GetComponentData<GamePrefabsSingleton>(gamePrefabsContainerEntity).asteroidPrefab;

            // Grab the number of asteroids to create from the GameSettingsSingleton
            Entity gameSettingsEntity = GetSingletonEntity<GameSettingsSingleton>();
            GameSettingsSingleton gameSettings = EntityManager.GetComponentData<GameSettingsSingleton>(gameSettingsEntity);

            for (int i = 0; i < gameSettings.asteroidsCount; i++)
            {
                Entity asteroid = EntityManager.Instantiate(asteroidPrefab);

                MovementComponent movementComponent = RandomMovementComponent.Create(random, gameSettings.asteroidSpeedRange, gameSettings.asteroidSpawnRange, gameSettings.asteroidDirectionRange);
                RotatorComponent rotatorComponent = InitializeAsteroidRotation(gameSettings, movementComponent);
                EntityManager.AddComponentData(asteroid, movementComponent);
                EntityManager.AddComponentData(asteroid, rotatorComponent);
            }
        }
        private RotatorComponent InitializeAsteroidRotation(GameSettingsSingleton gameSettings, MovementComponent movementComponent)
        {
            float rotationSpeed = random.NextFloat(gameSettings.asteroidRotationSpeedRange.x, gameSettings.asteroidRotationSpeedRange.y);
            RotatorConfig rotatorConfig = new RotatorConfig(rotationSpeed);
            RotatorComponent rotatorComponent = new RotatorComponent(rotatorConfig, quaternion.identity, movementComponent.Direction);
            return rotatorComponent;
        }
    }
}
