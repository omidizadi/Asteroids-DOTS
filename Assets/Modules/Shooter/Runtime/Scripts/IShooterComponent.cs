using DefaultNamespace.Configs;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
namespace DefaultNamespace.Components
{
    /// <summary>
    /// An interface for the shooter components that is only used to expose the necessary methods and properties.
    /// </summary>
    public interface IShooterComponent
    {
        public float3 Position { get; set; }
        public quaternion Rotation { get; set; }
        public void Update(float3 position, quaternion rotation);
    }
}
