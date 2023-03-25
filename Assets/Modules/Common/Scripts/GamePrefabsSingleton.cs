using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GamePrefabsSingleton : IComponentData
{
    [Header("Spaceships")]
    public Entity spaceshipEnemyPrefab;
    public Entity spaceshipPlayerPrefab;

    [Header("Bullets")]
    public Entity bulletEnemyPrefab;
    public Entity bulletPrefab;

    [Header("Asteroids")]
    public Entity asteroidPrefab;

    [Header("PowerUps")]
    public Entity powerUpShieldPrefab;
    public Entity powerUpTripleShotPrefab;
}
