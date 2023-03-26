using DefaultNamespace;
using Unity.Entities;
namespace Modules.PowerUp.Runtime.Scripts
{
    [GenerateAuthoringComponent]
    public struct PowerUpTag : IComponentData
    {
        public PowerUpType powerUpType;
    }
}
