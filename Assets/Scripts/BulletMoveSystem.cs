using Unity.Entities;
using Unity.Transforms;
namespace DefaultNamespace
{
    public class BulletMoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<OldBulletEntity, Translation>()
                .ForEach((Entity bulletEntity, ref Translation translation) =>
                {
                    OldBulletEntity oldBullet = EntityManager.GetComponentData<OldBulletEntity>(bulletEntity);
                    oldBullet.position += oldBullet.direction * oldBullet.speed * Time.DeltaTime;
                    translation.Value = oldBullet.position;
                    EntityManager.SetComponentData(bulletEntity, oldBullet);
                    
                    //TODO: replace magic numbers
                    if (oldBullet.position.x > 1000 || oldBullet.position.x < -1000 || oldBullet.position.y > 1000 || oldBullet.position.y < -1000)
                    {
                        PostUpdateCommands.DestroyEntity(bulletEntity);
                    }
                });
        }
    }
}
