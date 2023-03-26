using Modules.Movement.Runtime.Scripts;
using Unity.Mathematics;
namespace Modules.Common.Scripts
{
    /// <summary>
    /// Provides a static method to create a random movement component
    /// </summary>
    public static class RandomMovementComponent
    {
        /// <summary>
        /// Creates a random movement component
        /// </summary>
        /// <param name="random">The random number generator</param>
        /// <param name="speedRange">Range of the random speed</param>
        /// <param name="positionRange">Range of the random initial position</param>
        /// <param name="directionRange">Range of the random direction</param>
        /// <param name="acceleration">The acceleration of the movement</param>
        /// <param name="friction">The friction of the movement</param>
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
