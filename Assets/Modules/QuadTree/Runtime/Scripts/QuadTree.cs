using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public struct Quadtree
{
    private int maxDepth;
    private int maxObjects;
    private int depth;
    private float2 position;
    private float2 size;
    private bool nodesCreated;

    public Quadtree(int depth, int maxDepth, int maxObjects, float2 position, float2 size)
    {
        this.depth = depth;
        this.maxDepth = maxDepth;
        this.maxObjects = maxObjects;
        this.position = position;
        this.size = size;
        nodesCreated = false;
    }

    public void Insert(NativeArray<Quadtree> nodes, NativeArray<Entity> objects, Entity entity, float2 position)
    {
        if (nodesCreated)
        {
            int index = GetIndex(position);
            if (index != -1)
            {
                nodes[index].Insert(nodes, objects, entity, position);
                return;
            }
        }

        objects[objects.Length - 1] = entity;

        if (objects[objects.Length - 1] != Entity.Null && depth < maxDepth)
        {
            if (!nodesCreated)
                Split(nodes);

            int i = 0;
            while (i < objects.Length)
            {
                int index = GetIndex(position);
                if (index != -1)
                {
                    nodes[index].Insert(nodes, objects, entity, position);
                    objects[i] = Entity.Null;
                }
                i++;
            }
        }
    }

    public NativeList<Entity> Retrieve(NativeList<Entity> result, NativeArray<Quadtree> nodes, NativeArray<Entity> objects, float2 position)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            result.Add(objects[i]);
        }

        if (nodesCreated)
        {
            int index = GetIndex(position);
            if (index != -1)
            {
                nodes[index].Retrieve(result, nodes, objects, position);
            }
            else
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Retrieve(result, nodes, objects, position);
                }
            }
        }

        return result;
    }

    private void Split(NativeArray<Quadtree> nodes)
    {
        float2 halfSize = size / 2f;
        nodesCreated = true;
        nodes[0] = new Quadtree(depth + 1, maxDepth, maxObjects, position + new float2(0, halfSize.y), halfSize);
        nodes[1] = new Quadtree(depth + 1, maxDepth, maxObjects, position + new float2(halfSize.x, halfSize.y), halfSize);
        nodes[2] = new Quadtree(depth + 1, maxDepth, maxObjects, position + new float2(halfSize.x, 0), halfSize);
        nodes[3] = new Quadtree(depth + 1, maxDepth, maxObjects, position, halfSize);
    }

    private int GetIndex(float2 position)
    {
        int result = -1;
        float2 center = position + size / 2f;
        bool top = position.y < center.y && position.y + size.y < center.y;
        bool bottom = position.y > center.y;
        bool left = position.x < center.x && position.x + size.x < center.x;
        bool right = position.x > center.x;

        if (top)
        {
            if (left)
                result = 3;
            else if
                (right)
                result = 2;
        }
        else if (bottom)
        {
            if (left)
                result = 0;
            else if (right)
                result = 1;
        }
        return result;
    }
}
