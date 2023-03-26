using System;
using System.Collections.Generic;
using DefaultNamespace.Components;
using DefaultNamespace.Configs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
namespace DefaultNamespace.Systems
{
    /// <summary>
    /// Responsible for firing the shooter components based on their auto fire mode
    /// </summary>
    public class ShooterSystem : ComponentSystem
    {
        private InputAction fireInputAction;

        protected override void OnCreate()
        {
            //Player Fire Input
            fireInputAction = new InputAction("fire", binding: "<Mouse>/leftButton");
            fireInputAction.Enable();
        }
        protected override void OnUpdate()
        {
            Entities
                .WithAll<ShooterComponent, ShooterConfig, Translation, Rotation>()
                .ForEach((Entity entity, ref ShooterComponent shooterComponent, ref ShooterConfig config, ref Translation translation, ref Rotation rotation) =>
                {
                    shooterComponent.SetPosition(translation.Value);
                    shooterComponent.SetRotation(rotation.Value);
                    shooterComponent.Shoot(EntityManager, config, Time.DeltaTime, fireInputAction.triggered);
                });
        }

        protected override void OnDestroy()
        {
            fireInputAction.Disable();
        }
    }
}
