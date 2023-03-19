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
                    
                    //check if the bullet is out of the screen, destroy it
                    if (bullet.position.x > 100 || bullet.position.x < -100 || bullet.position.y > 100 || bullet.position.y < -100)
                    {
                        PostUpdateCommands.DestroyEntity(bulletEntity);
                    }
                });
        }
    }
}
