using Unity.Entities;
namespace Modules.Rotation.Runtime.Scripts
{
    /// <summary>
    /// Responsible for rotating the entity.
    /// </summary>
    public class RotationSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<RotatorComponent, Unity.Transforms.Rotation>()
                .ForEach((ref RotatorComponent rotatorComponent, ref Unity.Transforms.Rotation rotation) =>
                {
                    rotation.Value = rotatorComponent.Rotate(Time.DeltaTime);
                });
        }
    }
}
