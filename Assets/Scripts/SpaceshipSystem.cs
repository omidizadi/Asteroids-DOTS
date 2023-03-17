using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipSystem : ComponentSystem
{
    private InputAction moveAction;
    private InputAction fireAction;

    protected override void OnCreate()
    {
        // Create input actions for move and fire
        moveAction = new InputAction("move", binding: "<Keyboard>/wasd");
        fireAction = new InputAction("fire", binding: "<Mouse>/leftButton");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        // Enable input actions
        moveAction.Enable();
        fireAction.Enable();
    }

    protected override void OnDestroy()
    {
        // Disable input actions
        moveAction.Disable();
        fireAction.Disable();
    }

    protected override void OnUpdate()
    {
        Entities.WithAll<SpaceshipEntity, Translation>().ForEach((Entity entity, ref Translation translation) =>
        {
            // Check if move input is active
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            if (math.length(moveInput) > 0f)
            {
                Debug.Log("Move input: " + moveInput);
                float speed = 10f; // Set your desired speed
                translation.Value += new float3(moveInput.x, moveInput.y, 0) * speed * Time.DeltaTime;
            }

            // Check if fire input is active
            if (fireAction.triggered)
            {
                Debug.Log("Fire input!");
            }
        });
    }
}
