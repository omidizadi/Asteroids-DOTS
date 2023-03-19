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
                //Vector3.Distance is not a good idea for performance, is it the same case for math.distance?
                float distance = math.distance(spaceship.position, asteroid.position);
                //TODO: remove the magic number
                if (distance < 40f)
                {
                    PostUpdateCommands.DestroyEntity(asteroidEntity);
                    PostUpdateCommands.DestroyEntity(spaceshipEntity);
                    defeatPanel.enabled = true;
                }
            });
        }
    }
}
