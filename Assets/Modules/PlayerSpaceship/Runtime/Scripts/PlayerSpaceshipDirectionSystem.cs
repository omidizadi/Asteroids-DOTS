using DefaultNamespace;
using Modules.Mover.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private Rotation LookAtMousePosition(Translation translation)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = translation.Value.z;
        float3 direction = (float3)worldPosition - translation.Value;
        Quaternion lookRotation = Quaternion.LookRotation(direction, -Vector3.forward);
        return new Rotation { Value = lookRotation };
    }
}
