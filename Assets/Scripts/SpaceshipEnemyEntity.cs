using Unity.Entities;
using Unity.Mathematics;
namespace DefaultNamespace
{
    [GenerateAuthoringComponent]
    public struct SpaceshipEnemyEntity : IComponentData
    {
        public float3 position;
        public float3 direction;
    }
}
