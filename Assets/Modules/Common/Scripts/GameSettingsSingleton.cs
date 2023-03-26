using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace Modules.Common.Scripts
{
    [GenerateAuthoringComponent]
    public struct GameSettingsSingleton : IComponentData
    {
        public int asteroidsCount;
        public float2 asteroidRotationSpeedRange;
        public float2 asteroidSpeedRange;
        public float2 asteroidDirectionRange;
        public float2 asteroidSpawnRange;
        public float asteroidMinScale;
    }
}
