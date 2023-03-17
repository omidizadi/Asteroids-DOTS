using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GamePrefabsContainerEntity : IComponentData
{
    public Entity spaceshipPrefab;
    public Entity bulletPrefab;
}
