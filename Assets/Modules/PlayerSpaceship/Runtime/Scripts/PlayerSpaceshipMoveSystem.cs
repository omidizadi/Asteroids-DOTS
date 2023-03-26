using Modules.Movement.Runtime.Scripts;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Modules.PlayerSpaceship.Runtime.Scripts
{
    /// <summary>
    /// Responsible for moving the player spaceship using the keyboard.
    /// </summary>
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
                    movementComponent.UpdateConfig(movementConfig);
                    movementComponent.UpdateDirection(moveAction.ReadValue<Vector2>());
                });
        }
        protected override void OnDestroy()
        {
            moveAction.Disable();
        }
    }
}
