using Unity.Entities;
using Unity.Transforms;
using UnityEngine.InputSystem;
namespace Modules.Shooter.Runtime.Scripts
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
                .WithAll<ShooterComponent, ShooterConfig, Translation, Unity.Transforms.Rotation>()
                .ForEach((Entity entity, ref ShooterComponent shooterComponent, ref ShooterConfig config, ref Translation translation, ref Unity.Transforms.Rotation rotation) =>
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
