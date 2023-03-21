using DefaultNamespace;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SpaceshipEntity : IComponentData
{
    public float3 position;
    public quaternion rotation;
    public PowerUpType powerUpType;
}
