using Unity.Entities;
namespace DefaultNamespace
{
    [GenerateAuthoringComponent]
    public struct PowerUpEntity : IComponentData
    {
        public PowerUpType powerUpType;
    }
}
