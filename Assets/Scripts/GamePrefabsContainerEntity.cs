using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GamePrefabsContainerEntity : IComponentData
{
    public Entity spaceshipEnemyPrefab;
    public Entity bulletEnemyPrefab;

    public Entity asteroidPrefab;
    
    public Entity bulletPrefab;
    public Entity powerUpShieldPrefab;
    public Entity powerUpTripleShotPrefab;
    
}
