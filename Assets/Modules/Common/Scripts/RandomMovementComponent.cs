using Modules.Mover.Runtime.Scripts;
using Unity.Mathematics;
namespace Modules.Common.Scripts
{
    public static class RandomMovementComponent
    {
        public static MovementComponent Create(Random random, float2 speedRange, float2 positionRange, float2 directionRange, float acceleration = 0, float friction = 0)
        {
            // Randomize the asteroid's speed
            float speed = random.NextFloat(speedRange.x, speedRange.y);

            // Randomize the asteroid's position
            if (random.NextInt(0, 2) == 0)
            {
                positionRange.x *= -1;
            }
            if (random.NextInt(0, 2) == 0)
            {
                positionRange.y *= -1;
            }
            float3 asteroidPosition = new float3(random.NextFloat(positionRange.x, positionRange.y), random.NextFloat(positionRange.x, positionRange.y), 0f);

            // Randomize the asteroid's direction
            float3 asteroidDirection = math.normalize(new float3(random.NextFloat(directionRange.x, directionRange.y), random.NextFloat(directionRange.x, directionRange.y), 0f));

            // Create the movement component
            MovementConfig movementConfig = new MovementConfig(speed, acceleration, friction);
            MovementComponent movementComponent = new MovementComponent(movementConfig, asteroidPosition, asteroidDirection);
            return movementComponent;
        }
    }
}
