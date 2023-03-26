using Modules.Common.Scripts;
using Modules.Movement.Runtime.Scripts;
using Modules.Rotation.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
namespace Modules.Asteroids.Runtime.Scripts
{
    /// <summary>
    /// Responsible for creating the asteroids at the start of the game
    /// </summary>
    public class AsteroidsCreatorSystem : ComponentSystem
    {
        private bool asteroidsCreated;
        private Random random;



        protected override void OnUpdate()
        {
            // Create the asteroids only once
            if (!asteroidsCreated)
            {
                CreateAsteroids();
                asteroidsCreated = true;
            }
        }
        private void CreateAsteroids()
        {
            // Create a random number generator
            random = new Random((uint)Time.ElapsedTime + 1);

            // Grab the asteroid prefab from the GamePrefabsSingleton to instantiate
            Entity gamePrefabsContainerEntity = GetSingletonEntity<GamePrefabsSingleton>();
            Entity asteroidPrefab = EntityManager.GetComponentData<GamePrefabsSingleton>(gamePrefabsContainerEntity).asteroidPrefab;

            // Grab the number of asteroids to create from the GameSettingsSingleton
            Entity gameSettingsEntity = GetSingletonEntity<GameSettingsSingleton>();
            GameSettingsSingleton gameSettings = EntityManager.GetComponentData<GameSettingsSingleton>(gameSettingsEntity);

            for (int i = 0; i < gameSettings.asteroidsCount; i++)
            {
                Entity asteroid = EntityManager.Instantiate(asteroidPrefab);

                // Create a random movement component for the asteroid
                MovementComponent movementComponent = RandomMovementComponent.Create(random, gameSettings.asteroidSpeedRange, gameSettings.asteroidSpawnRange, gameSettings.asteroidDirectionRange);

                // Create a random rotation component for the asteroid
                RotatorComponent rotatorComponent = InitializeAsteroidRotation(gameSettings, movementComponent);

                // Set the asteroid's position and rotation
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
