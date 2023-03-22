using Unity.Entities;
using Unity.Transforms;
namespace DefaultNamespace
{
    public class BulletEnemyMoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<BulletEnemyEntity, Translation>()
                .ForEach((Entity bulletEntity, ref Translation translation) =>
                {
                    BulletEnemyEntity bullet = EntityManager.GetComponentData<BulletEnemyEntity>(bulletEntity);
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
