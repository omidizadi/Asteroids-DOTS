using Unity.Entities;
using Unity.Mathematics;
namespace Modules.Rotation.Runtime.Scripts
{
    /// <summary>
    /// An entity with this component will rotate facing its direction
    /// </summary>
    [GenerateAuthoringComponent]
    public struct RotatorComponent : IComponentData
    {
        //config
        private RotatorConfig config;

        //logic
        private quaternion rotation;
        private float3 direction;

        public RotatorComponent(RotatorConfig config, quaternion rotation, float3 direction)
        {
            this.config = config;
            this.rotation = rotation;
            this.direction = direction;
        }

        public quaternion Rotate(float deltaTime)
        {
            rotation = math.mul(rotation, quaternion.AxisAngle(math.cross(direction, new float3(0, 0, 1)), config.speed * deltaTime));
            return rotation;
        }
    }
}
