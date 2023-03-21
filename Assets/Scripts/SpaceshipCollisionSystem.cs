using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace DefaultNamespace
{
    [UpdateAfter(typeof(SpaceshipSystem))]
    [UpdateAfter(typeof(AsteroidsSystem))]
    public class SpaceshipCollisionSystem : ComponentSystem
    {
        private Canvas defeatPanel;

        protected override void OnUpdate()
        {
            if (!HasSingleton<SpaceshipEntity>())
            {
                return;
            }

            if (defeatPanel == null)
            {
                GameObject canvas = GameObject.Find("DefeatCanvas");
                defeatPanel = canvas.GetComponent<Canvas>();
            }
            Entity spaceshipEntity = GetSingletonEntity<SpaceshipEntity>();
            Entities.WithAll<AsteroidEntity>().ForEach(asteroidEntity =>
            {
                SpaceshipEntity spaceship = EntityManager.GetComponentData<SpaceshipEntity>(spaceshipEntity);
                if (spaceship.powerUpType == PowerUpType.Shield)
                {
                    return;
                }
                AsteroidEntity asteroid = EntityManager.GetComponentData<AsteroidEntity>(asteroidEntity);
                float distance = math.length(spaceship.position - asteroid.position);
                //TODO: remove the magic number
                if (distance < 20)
                {
                    PostUpdateCommands.DestroyEntity(asteroidEntity);
                    PostUpdateCommands.DestroyEntity(spaceshipEntity);
                    defeatPanel.enabled = true;
                }
            });
        }
    }
}
