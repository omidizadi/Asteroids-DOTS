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
                AsteroidEntity asteroid = EntityManager.GetComponentData<AsteroidEntity>(asteroidEntity);
                SpaceshipEntity spaceship = EntityManager.GetComponentData<SpaceshipEntity>(spaceshipEntity);
                float distance = math.distance(asteroid.position, spaceship.position);
                //TODO: remove the magic number
                if (distance < 60f)
                {
                    PostUpdateCommands.DestroyEntity(asteroidEntity);
                    PostUpdateCommands.DestroyEntity(spaceshipEntity);
                    defeatPanel.enabled = true;
                }
            });
        }
    }
}
