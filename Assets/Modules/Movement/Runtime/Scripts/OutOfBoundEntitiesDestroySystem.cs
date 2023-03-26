using Unity.Entities;
using Unity.Transforms;
namespace Modules.Mover.Runtime.Scripts
{
    public class OutOfBoundEntitiesDestroySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<MovementComponent, Translation>()
                .ForEach((Entity entity, ref Translation translation) =>
                {
                    if (translation.Value.x > 700 || translation.Value.x < -700 || translation.Value.y > 700 || translation.Value.y < -700)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                });
        }
    }
}
