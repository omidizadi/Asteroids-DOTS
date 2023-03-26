using Unity.Entities;
using Unity.Transforms;
namespace Modules.Mover.Runtime.Scripts
{
    /// <summary>
    /// Responsible for destroying entities that are out of bounds to save performance.
    /// </summary>
    public class OutOfBoundEntitiesDestroySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<MovementComponent, Translation>()
                .ForEach((Entity entity, ref Translation translation) =>
                {
                    const float maxDistance = 650;
                    if (translation.Value.x > maxDistance || translation.Value.x < -maxDistance || translation.Value.y > maxDistance || translation.Value.y < -maxDistance)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                });
        }
    }
}
