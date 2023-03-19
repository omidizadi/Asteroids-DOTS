using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
    [GenerateAuthoringComponent]
    public struct AsteroidEntity : IComponentData
    {
        public int asteroidIndex;
        public float3 position;
        public quaternion rotation;
        public float3 direction;
        public float speed;
    }
}
