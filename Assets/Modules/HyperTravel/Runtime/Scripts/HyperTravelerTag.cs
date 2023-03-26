using Unity.Entities;
namespace Modules.HyperTravel.Runtime.Scripts
{
    /// <summary>
    /// Any entity with this tag will be able to travel out of the screen and back in.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct HyperTravelerTag : IComponentData
    {
    }
}
