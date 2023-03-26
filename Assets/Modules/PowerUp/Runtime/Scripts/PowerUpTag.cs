using DefaultNamespace;
using Unity.Entities;
namespace Modules.PowerUp.Runtime.Scripts
{
    /// <summary>
    /// The tag for the power up.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct PowerUpTag : IComponentData
    {
        public PowerUpType powerUpType;
    }
}
