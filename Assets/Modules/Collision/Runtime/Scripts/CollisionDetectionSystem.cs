using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Responsible for detecting collisions between entities with a CollisionComponent
/// </summary>
public class CollisionDetectionSystem : ComponentSystem
{
    private float collisionDetectionDelay = 1f;
    private float timeUntilCollisionDetection = 0f;
    protected override void OnUpdate()
    {
        //Sets an initial delay before collision detection starts
        if (timeUntilCollisionDetection < collisionDetectionDelay)
        {
            timeUntilCollisionDetection += Time.DeltaTime;
            return;
        }

        Entities
            .WithAll<CollisionComponent>()
            .ForEach((Entity entity, ref Translation translation, ref CollisionComponent collision) =>
            {
                float radius = collision.Radius;

                //Get all entities with a CollisionComponent
                EntityQuery entityQuery = GetEntityQuery(ComponentType.ReadOnly<CollisionComponent>(), ComponentType.ReadOnly<Translation>());
                NativeArray<Entity> entitiesInRange =  World.GetOrCreateSystem<QuadtreeSystem>().Retrieve(translation.Value.xy);

                foreach (Entity otherEntity in entitiesInRange)
                {
                    if (entity != otherEntity && EntityManager.HasComponent<CollisionComponent>(otherEntity))
                    {
                        float2 otherPosition = EntityManager.GetComponentData<Translation>(otherEntity).Value.xy;
                        CollisionComponent otherCollision = EntityManager.GetComponentData<CollisionComponent>(otherEntity);
                        float otherRadius = otherCollision.Radius;
                        float distance = math.distance(translation.Value.xy, otherPosition);

                        //If the distance between the two entities is less than the sum of their radius, they are colliding
                        if (distance <= radius + otherRadius)
                        {
                            collision.UpdateCollisionStatus(EntityManager, entity, otherEntity);
                        }
                    }
                }

                entitiesInRange.Dispose();
            });
    }
}
