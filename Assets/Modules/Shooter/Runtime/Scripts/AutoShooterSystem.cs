using Modules.PlayerSpaceship.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace Modules.Shooter.Runtime.Scripts
{
    /// <summary>
    /// Responsible for shooting the auto fire mode shooter components.
    /// </summary>
    public class AutoShooterSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<ShooterComponent, ShooterConfig, Translation, Unity.Transforms.Rotation>()
                .ForEach((Entity entity, ref ShooterComponent shooterComponent, ref ShooterConfig config, ref Translation translation, ref Unity.Transforms.Rotation rotation) =>
                {
                    if (config.fireMode == FireMode.Auto)
                    {
                        quaternion newRotation = CalculateSpaceshipRotation(rotation, config, translation);
                        shooterComponent.ShootAuto(
                            EntityManager,
                            config,
                            translation.Value,
                            newRotation,
                            Time.DeltaTime);
                    }
                });
        }

        private quaternion CalculateSpaceshipRotation(Unity.Transforms.Rotation rotation, ShooterConfig config, Translation translation)
        {
            quaternion newRotation = rotation.Value;
            if (config.autoFireTarget == AutoFireTarget.PlayerSpaceship)
            {
                //check if the player spaceship singleton exists
                if (HasSingleton<PlayerSpaceshipTag>())
                {
                    Entity playerSpaceshipEntity = GetSingletonEntity<PlayerSpaceshipTag>();
                    Translation playerSpaceshipTranslation = EntityManager.GetComponentData<Translation>(playerSpaceshipEntity);
                    float3 direction = playerSpaceshipTranslation.Value - translation.Value;
                    newRotation = quaternion.LookRotation(direction, -math.forward());
                }
            }
            return newRotation;
        }
    }
}
