﻿using Unity.Entities;
namespace Modules.Mover.Runtime.Scripts
{
    [GenerateAuthoringComponent]
    public struct MovementConfig : IComponentData
    {
        public float speed;
        public float acceleration;
        public float friction;
        
        public MovementConfig(float speed, float acceleration, float friction)
        {
            this.speed = speed;
            this.acceleration = acceleration;
            this.friction = friction;
        }
    }
}