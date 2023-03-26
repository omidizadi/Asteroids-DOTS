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

                MovementComponent movementComponent = InitializeAsteroidMovement(gameSettings);
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

        private MovementComponent InitializeAsteroidMovement(GameSettingsSingleton gameSettings)
        {
            // Randomize the asteroid's speed
            float2 speedRange = gameSettings.asteroidSpeedRange;
            float asteroidSpeed = random.NextFloat(speedRange.x, speedRange.y);

            // Randomize the asteroid's position
            float2 spawnRange = gameSettings.asteroidSpawnRange;
            if (random.NextInt(0, 2) == 0)
            {
                spawnRange.x *= -1;
            }
            if (random.NextInt(0, 2) == 0)
            {
                spawnRange.y *= -1;
            }
            float3 asteroidPosition = new float3(random.NextFloat(spawnRange.x, spawnRange.y), random.NextFloat(spawnRange.x, spawnRange.y), 0f);

            // Randomize the asteroid's direction
            float2 directionRange = gameSettings.asteroidDirectionRange;
            float3 asteroidDirection = math.normalize(new float3(random.NextFloat(directionRange.x, directionRange.y), random.NextFloat(directionRange.x, directionRange.y), 0f));

            // Create the movement component
            MovementConfig movementConfig = new MovementConfig(asteroidSpeed, 0f, 0f);
            MovementComponent movementComponent = new MovementComponent(movementConfig, asteroidPosition, asteroidDirection);
            return movementComponent;
        }
    }
}
