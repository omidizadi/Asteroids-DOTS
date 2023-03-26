using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Modules.PlayerSpaceship.Runtime.Scripts
{
    /// <summary>
    /// Responsible for rotating the player spaceship towards the mouse position.
    /// </summary>
    public class PlayerSpaceshipDirectionSystem : ComponentSystem
    {
        private Camera mainCamera;
        protected override void OnUpdate()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            Entities
                .WithAll<PlayerSpaceshipTag, Translation>()
                .ForEach((Entity entity, ref Translation translation) =>
                {
                    EntityManager.SetComponentData(entity, LookAtMousePosition(translation));
                });
        }

        private Unity.Transforms.Rotation LookAtMousePosition(Translation translation)
        {
            // Get the mouse position in world space
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = translation.Value.z;

            // Calculate the direction and rotation
            float3 direction = (float3)worldPosition - translation.Value;
            Quaternion lookRotation = Quaternion.LookRotation(direction, -Vector3.forward);
            return new Unity.Transforms.Rotation { Value = lookRotation };
        }
    }
}
