using DefaultNamespace.Components;
using Unity.Entities;
namespace DefaultNamespace.Configs
{
    /// <summary>
    /// Keeps the configuration for the shooter component
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ShooterConfig : IComponentData
    {
        public Entity bulletEntityPrefab;
        public AutoFireTarget autoFireTarget;
        public FireMode fireMode;
        public int bulletsCount;
        public float bulletSpeed;
        public float timeBetweenShots;
    }
}
