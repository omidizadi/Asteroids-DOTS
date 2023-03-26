using DefaultNamespace.Components;
using DefaultNamespace.Configs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.InputSystem;
namespace DefaultNamespace.Systems
{
    /// <summary>
    /// Responsible for firing the shooter components based on their auto fire mode
    /// </summary>
    public class ManualShooterSystem : ComponentSystem
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
                    if (config.fireMode == FireMode.Manual)
                    {
                        shooterComponent.ShootManual(
                            EntityManager,
                            config,
                            translation.Value,
                            rotation.Value,
                            fireInputAction.triggered);
                    }
                });
        }

        protected override void OnDestroy()
        {
            fireInputAction.Disable();
        }
    }
}
