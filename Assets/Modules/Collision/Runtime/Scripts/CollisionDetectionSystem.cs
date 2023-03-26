using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class CollisionDetectionSystem : ComponentSystem
{
    private float collisionDetectionDelay = 1f;
    private float timeUntilCollisionDetection = 0f;
    protected override void OnUpdate()
    {
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
                // var entitiesInRange = World.GetOrCreateSystem<QuadtreeSystem>().Retrieve(translation.Value.xy);
                EntityQuery entityQuery = GetEntityQuery(ComponentType.ReadOnly<CollisionComponent>(), ComponentType.ReadOnly<Translation>());
                NativeArray<Entity> entitiesInRange = entityQuery.ToEntityArray(Allocator.TempJob);
                for (int i = 0; i < entitiesInRange.Length; i++)
                {
                    Entity otherEntity = entitiesInRange[i];
                    if (entity != otherEntity && EntityManager.HasComponent<CollisionComponent>(otherEntity))
                    {
                        float2 otherPosition = EntityManager.GetComponentData<Translation>(otherEntity).Value.xy;
                        CollisionComponent otherCollision = EntityManager.GetComponentData<CollisionComponent>(otherEntity);
                        float otherRadius = otherCollision.Radius;
                        float distance = math.distance(translation.Value.xy, otherPosition);
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
