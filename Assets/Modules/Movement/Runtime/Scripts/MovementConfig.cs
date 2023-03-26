using Unity.Entities;
namespace Modules.Mover.Runtime.Scripts
{
    [GenerateAuthoringComponent]
    public struct MovementConfig : IComponentData
    {
        
        public float speed;
        public float acceleration;
        public float friction;
    }
}
