using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace Modules.Common.Scripts
{
    /// <summary>
    /// Provides access to all settings in the game
    /// </summary>
    [GenerateAuthoringComponent]
    public struct GameSettingsSingleton : IComponentData
    {
        [Header("Asteroids Settings")]
        public int asteroidsCount;
        public float2 asteroidRotationSpeedRange;
        public float2 asteroidSpeedRange;
        public float2 asteroidDirectionRange;
        public float2 asteroidSpawnRange;
        public float asteroidMinScale;

        [Header("PowerUps Settings")]
        public float2 powerUpsSpawnRange;

        [Header("Enemy Spaceship Settings")]
        public float2 enemySpaceshipDirectionRange;
        public float2 enemySpaceshipSpawnRange;
        public float2 enemySpaceshipSpeedRange;
    }
}
