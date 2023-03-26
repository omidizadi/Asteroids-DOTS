using Unity.Entities;
using UnityEngine;

/// <summary>
/// Provides access to all prefabs in the game
/// </summary>
[GenerateAuthoringComponent]
public struct GamePrefabsSingleton : IComponentData
{
    [Header("Spaceships")]
    public Entity spaceshipEnemyPrefab;

    [Header("Asteroids")]
    public Entity asteroidPrefab;

    [Header("PowerUps")]
    public Entity powerUpShieldPrefab;
    public Entity powerUpTripleShotPrefab;
    public Entity powerUpBulletSpeedPrefab;
}
