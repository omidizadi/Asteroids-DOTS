using Unity.Entities;
using Unity.Transforms;
namespace Modules.Rotator.Runtime.Scripts
{
    public class RotationSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<RotatorComponent, Rotation>()
                .ForEach((ref RotatorComponent rotatorComponent, ref Rotation rotation) =>
                {
                    rotation.Value = rotatorComponent.Rotate(Time.DeltaTime);
                });
        }
    }
}
