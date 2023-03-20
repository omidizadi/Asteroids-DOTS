using Unity.Entities;
using Unity.Transforms;
namespace DefaultNamespace
{
    public class BulletMoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<BulletEntity, Translation>()
                .ForEach((Entity bulletEntity, ref Translation translation) =>
                {
                    BulletEntity bullet = EntityManager.GetComponentData<BulletEntity>(bulletEntity);
                    bullet.position += bullet.direction * bullet.speed * Time.DeltaTime;
                    translation.Value = bullet.position;
                    EntityManager.SetComponentData(bulletEntity, bullet);
                    
                    //TODO: replace magic numbers
                    if (bullet.position.x > 1000 || bullet.position.x < -1000 || bullet.position.y > 1000 || bullet.position.y < -1000)
                    {
                        PostUpdateCommands.DestroyEntity(bulletEntity);
                    }
                });
        }
    }
}
