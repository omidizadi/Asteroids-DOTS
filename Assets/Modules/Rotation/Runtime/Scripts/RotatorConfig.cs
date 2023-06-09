﻿using Unity.Entities;
namespace Modules.Rotation.Runtime.Scripts
{
    /// <summary>
    /// The configuration for the rotator component.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct RotatorConfig : IComponentData
    {
        public float speed;

        public RotatorConfig(float speed)
        {
            this.speed = speed;
        }
    }
}
