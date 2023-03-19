using Unity.Entities;
using Unity.Mathematics;
namespace DefaultNamespace
{
    [GenerateAuthoringComponent]
    public struct BulletEntity: IComponentData
    {
        public float3 position;
        public float3 direction;
        public float speed;
    }
}
