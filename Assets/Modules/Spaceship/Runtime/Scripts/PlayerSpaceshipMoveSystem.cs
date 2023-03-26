using Modules.Mover.Runtime.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Modules.Spaceship.Runtime.Scripts
{
    public class PlayerSpaceshipMoveSystem : ComponentSystem
    {
        private InputAction moveAction;

        protected override void OnCreate()
        {
            moveAction = new InputAction("move", binding: "<Keyboard>/wasd");
            moveAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            // Enable input actions
            moveAction.Enable();
        }
        protected override void OnUpdate()
        {
            Entities
                .WithAll<PlayerSpaceshipTag, MovementComponent, MovementConfig>()
                .ForEach((ref MovementComponent movementComponent, ref MovementConfig movementConfig) =>
                {
                    movementComponent.SetConfig(movementConfig);
                    movementComponent.SetDirection(moveAction.ReadValue<Vector2>());
                });
        }
        protected override void OnDestroy()
        {
            moveAction.Disable();
        }
    }
}
