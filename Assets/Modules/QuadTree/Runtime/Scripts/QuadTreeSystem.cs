using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(CollisionSystem))]
[BurstCompile]
public class QuadtreeSystem : JobComponentSystem
{
    private Quadtree quadtree;
    private NativeArray<Entity> entities;
    private NativeArray<Entity> objects;
    private NativeArray<Quadtree> nodes;

    protected override void OnCreate()
    {
        quadtree = new Quadtree(0, 5, 1000, new float2(-100, -100), new float2(200, 200));
        nodes = new NativeArray<Quadtree>(4, Allocator.Persistent);
        objects = new NativeArray<Entity>(10000, Allocator.Persistent);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var entityQuery = GetEntityQuery(ComponentType.ReadOnly<CollisionComponent>(), ComponentType.ReadOnly<Translation>());
        entities = entityQuery.ToEntityArray(Allocator.TempJob);
        var positions = entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        //create a native array of float2 positions from the positions array
        var positionsArray = new NativeArray<float2>(positions.Length, Allocator.TempJob);
        for (int i = 0; i < positions.Length; i++)
        {
            positionsArray[i] = positions[i].Value.xy;
        }

        var insertJob = new InsertJob(nodes, objects, entities, positionsArray, quadtree);
        var insertJobHandle = insertJob.Schedule(entities.Length, 64);

        insertJobHandle.Complete();
        entities.Dispose();
        positions.Dispose();
        positionsArray.Dispose();
        return JobHandle.CombineDependencies(insertJobHandle, inputDeps);
    }

    public NativeList<Entity> Retrieve(float2 position)
    {
        NativeList<Entity> newEntities = new NativeList<Entity>(Allocator.Temp);
        quadtree.Retrieve(newEntities, nodes, objects, position);
        return newEntities;
    }

    protected override void OnDestroy()
    {
        // Dispose of the NativeArray when the system is destroyed
        nodes.Dispose();
        objects.Dispose();
    }

    [BurstCompile]
    public struct InsertJob : IJobParallelFor
    {
        public NativeArray<Entity> entities;

        [NativeDisableParallelForRestriction] //TODO: This is a workaround for the NativeArray<Entity> not being blittable
        public NativeArray<Entity> objects;

        [NativeDisableParallelForRestriction] //TODO: This is a workaround for the NativeArray<Entity> not being blittable
        public NativeArray<Quadtree> nodes;
        public NativeArray<float2> positions;
        public Quadtree quadtree;

        public void Execute(int index)
        {
            quadtree.Insert(nodes, objects, entities[index], positions[index]);
        }

        public InsertJob(NativeArray<Quadtree> nodes, NativeArray<Entity> objects, NativeArray<Entity> entities, NativeArray<float2> positions, Quadtree quadtree)
        {
            this.entities = entities;
            this.positions = positions;
            this.quadtree = quadtree;
            this.nodes = nodes;
            this.objects = objects;
        }
    }
}
