using Unity.Entities;
using Unity.Transforms;
namespace Modules.Mover.Runtime.Scripts
{
    /// <summary>
    /// A system that updates the movement of entities with a <see cref="MovementComponent"/>.
    /// </summary>
    public class MovementSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<MovementComponent, Translation>()
                .ForEach((ref MovementComponent movementComponent, ref Translation translation) =>
                {
                    movementComponent.Move(Time.DeltaTime);
                    translation.Value = movementComponent.Position;
                });
        }
    }
}
