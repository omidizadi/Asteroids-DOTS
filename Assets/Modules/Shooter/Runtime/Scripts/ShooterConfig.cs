using System;
using DefaultNamespace.Components;
using Unity.Entities;
using UnityEngine;
namespace DefaultNamespace.Configs
{
    /// <summary>
    /// Keeps the configuration for the shooter component. Should be attached to the same entity as the <see cref="IShooterComponent"/>
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ShooterConfig : IComponentData
    {
        public Entity bulletEntityPrefab;
        public AutoFireMode autoFireMode;
        public float bulletSpeed;
        public float timeBetweenShots;
    }
}
