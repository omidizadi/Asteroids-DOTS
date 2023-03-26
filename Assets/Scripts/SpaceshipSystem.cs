using DefaultNamespace;
using Modules.Mover.Runtime.Scripts;
using Modules.Spaceship.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
public class SpaceshipSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<SpaceshipEntity, Translation, Rotation>()
            .ForEach((Entity entity, ref Translation translation, ref Rotation rotation) =>
            {
                ApplyRotation(ref rotation, translation);

                //Set the position and the rotation of the spaceship in the SpaceshipEntity component
                SpaceshipEntity spaceshipEntity = EntityManager.GetComponentData<SpaceshipEntity>(entity);
                spaceshipEntity.position = translation.Value;
                spaceshipEntity.rotation = rotation.Value;
                EntityManager.SetComponentData(entity, spaceshipEntity);
            });
    }

    private void ApplyRotation(ref Rotation rotation, Translation translation)
    {
        // the ship look at the mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        //TODO: retrieve camera from a singleton to avoid using Camera.main
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = translation.Value.z;
        float3 direction = (float3)worldPosition - translation.Value;
        Quaternion lookRotation = Quaternion.LookRotation(direction, -Vector3.forward);
        rotation.Value = lookRotation;
    }
}
