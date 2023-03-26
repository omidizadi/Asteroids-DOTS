using Unity.Entities;
using Unity.Mathematics;
namespace Modules.Rotator.Runtime.Scripts
{
    [GenerateAuthoringComponent]
    public struct RotatorConfig : IComponentData
    {
        public float speed;

        public RotatorConfig(float speed)
        {
            this.speed = speed;
        }
    }
}
