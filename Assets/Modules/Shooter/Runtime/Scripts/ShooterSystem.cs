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
            //TODO: somehow unify the query for all IShooterComponents
            Entities
                .WithAll<SingleShooterComponent, ShooterConfig, Translation, Rotation>()
                .ForEach((Entity entity, ref SingleShooterComponent singleShooterComponent, ref ShooterConfig config, ref Translation translation, ref Rotation rotation) =>
                {
                    singleShooterComponent.Update(translation.Value, rotation.Value);
                    singleShooterComponent.Shoot(EntityManager, config, Time.DeltaTime, fireInputAction.triggered);
                });
        }

        protected override void OnDestroy()
        {
            fireInputAction.Disable();
        }
    }
}
