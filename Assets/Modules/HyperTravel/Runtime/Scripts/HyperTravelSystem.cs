using Modules.Mover.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace Modules.HyperTravel.Runtime.Scripts
{
    /// <summary>
    /// Responsible for moving entities with the <see cref="HyperTravelerTag"/> tag out of the screen and back in.
    /// </summary>
    [UpdateAfter(typeof(MovementSystem))]
    public class HyperTravelSystem : ComponentSystem
    {
        private Camera mainCamera;
        protected override void OnUpdate()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            Entities
                .WithAll<HyperTravelerTag, MovementComponent>()
                .ForEach((ref MovementComponent movementComponent) =>
                {
                    
                    float3 position = movementComponent.Position;
                    
                    // Get the screen bounds
                    float screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
                    float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
                    float screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
                    float screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

                    if (position.y > screenTop)
                    {
                        position.y = screenBottom;
                    }
                    else if (position.y < screenBottom)
                    {
                        position.y = screenTop;
                    }

                    if (position.x < screenLeft)
                    {
                        position.x = screenRight;
                    }
                    else if (position.x > screenRight)
                    {
                        position.x = screenLeft;
                    }
                    movementComponent.UpdatePosition(position);
                });
        }
    }
}
