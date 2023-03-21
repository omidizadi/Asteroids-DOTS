using DefaultNamespace;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
public class SpaceshipSystem : ComponentSystem
{
    private InputAction moveAction;
    private InputAction fireAction;

    public float speed = 2.5f;    // speed of object
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
                ApplyVelocity(ref translation);
                DoHyperSpaceTravel(ref translation);
                ApplyRotation(ref rotation, translation);

                //Set the position and the rotation of the spaceship in the SpaceshipEntity component
                SpaceshipEntity spaceshipEntity = EntityManager.GetComponentData<SpaceshipEntity>(entity);
                spaceshipEntity.position = translation.Value;
                spaceshipEntity.rotation = rotation.Value;
                EntityManager.SetComponentData(entity, spaceshipEntity);

                // Check fire input
                if (fireAction.triggered)
                {
                    Entity gamePrefabsContainerEntity = GetSingletonEntity<GamePrefabsContainerEntity>();
                    Entity bulletPrefab = EntityManager.GetComponentData<GamePrefabsContainerEntity>(gamePrefabsContainerEntity).bulletPrefab;


                    CreateNewBullet(bulletPrefab, translation, rotation);
                    if (spaceshipEntity.powerUpType == PowerUpType.TripleShot)
                    {
                        CreateNewBullet(bulletPrefab, translation, rotation, 30);
                        CreateNewBullet(bulletPrefab, translation, rotation, -30);
                    }
                }
            });
    }
    private void CreateNewBullet(Entity bulletPrefab, Translation translation, Rotation rotation, int angle = 0)
    {
        Entity bullet = EntityManager.Instantiate(bulletPrefab);
        BulletEntity bulletEntity = EntityManager.GetComponentData<BulletEntity>(bullet);
        bulletEntity.position = translation.Value;
        bulletEntity.direction = math.mul(quaternion.AxisAngle(math.forward(), math.radians(angle)), math.forward(rotation.Value));
        //TODO: modify the speed of the bullet based on the spaceship's speed
        bulletEntity.speed = 200;
        EntityManager.SetComponentData(bullet, bulletEntity);
    }

    private void ApplyVelocity(ref Translation translation)
    {
        // Update object's position
        translation.Value += velocity;
    }
    private void DoHyperSpaceTravel(ref Translation translation)
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
    }

    private void ApplyRotation(ref Rotation rotation, Translation translation)
    {
        // the ship look at the mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        //TODO: retrieve camera from a singleton to avoid using Camera.main
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = translation.Value.z;
        float3 direction = (float3)worldPosition - translation.Value;
        Quaternion lookRotation = Quaternion.LookRotation(direction, -Vector3.forward);
        rotation.Value = lookRotation;
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
