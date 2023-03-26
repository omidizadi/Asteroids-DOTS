using Modules.Collision.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using UnityEngine;
namespace Modules.GameState.Runtime.Scripts
{
    [UpdateAfter(typeof(CollisionResolveSystem))]
    public class GameStateSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            //get the number of PlayerSpaceshipTag entities
            int playerSpaceshipCount = EntityManager.CreateEntityQuery(typeof(PlayerSpaceshipTag)).CalculateEntityCount();
            if (playerSpaceshipCount == 0)
            {
                GameObject.Find("DefeatCanvas").GetComponent<Canvas>().enabled = true;
                Debug.Log("Game Over");
            }
        }
    }
}
