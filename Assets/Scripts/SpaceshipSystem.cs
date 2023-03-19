using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
public class SpaceshipSystem : ComponentSystem
{
    private InputAction moveAction;
    private InputAction fireAction;

    public float speed = 2.5f;  // speed of object
    public float friction = 1.5f; // amount of friction to apply to slow down object
    private float3 velocity;
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
        Entities
            .WithAll<SpaceshipEntity, Translation, Rotation>()
            .ForEach((Entity entity, ref Translation translation, ref Rotation rotation) =>
            {
                CalculateVelocity();
                translation = ApplyVelocity(translation);
                translation = DoHyperSpaceTravel(translation);
                rotation = ApplyRotation(rotation);

                // Check fire input
                if (fireAction.triggered)
                {
                    Debug.Log("Fire input!");
                }
            });
    }

    private Translation ApplyVelocity(Translation translation)
    {
        // Update object's position
        translation.Value += velocity;
        return translation;
    }
    private Translation DoHyperSpaceTravel(Translation translation)
    {
        //TODO: retrieve camera from a singleton to avoid using Camera.main
        if (Camera.main != null)
        {
            float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
            float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            if (translation.Value.y > screenTop)
            {
                translation.Value.y = screenBottom;
            }
            else if (translation.Value.y < screenBottom)
            {
                translation.Value.y = screenTop;
            }

            if (translation.Value.x < screenLeft)
            {
                translation.Value.x = screenRight;
            }
            else if (translation.Value.x > screenRight)
            {
                translation.Value.x = screenLeft;
            }
        }
        return translation;
    }

    private Rotation ApplyRotation(Rotation rotation)
    {
        // Rotate the spaceship to face the direction of movement
        if (math.length(velocity) > 0f)
        {
            float3 forward = new float3(0f, 0f, -1f);
            float3 direction = math.normalize(velocity);
            quaternion targetRotation = quaternion.LookRotation(direction, forward);
            rotation.Value = targetRotation;
        }
        return rotation;
    }

    private void CalculateVelocity()
    {
        // Check move input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0f)
        {
            ApplyMovement(moveInput);
        }
        else
        {
            ApplyFriction();
            StopMovementIfVelocityIsVerySmall();
        }
    }

    private void ApplyMovement(Vector2 moveInput)
    {
        float3 move = new float3(moveInput.x, moveInput.y, 0f);
        float3 normalizedMove = math.normalize(move);
        velocity += normalizedMove * speed * Time.DeltaTime;
    }

    private void ApplyFriction()
    {
        if (math.length(velocity) > 0f)
        {
            velocity -= math.normalize(velocity) * friction * Time.DeltaTime;
        }
    }

    private void StopMovementIfVelocityIsVerySmall()
    {
        if (math.length(velocity) < 0.1f)
        {
            velocity = Vector3.zero;
        }
    }
}
