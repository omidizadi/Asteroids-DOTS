using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
namespace DefaultNamespace
{
    public struct AsteroidMoveJob : IJobFor
    {
        public float deltaTime;
        public NativeArray<AsteroidEntity> asteroidsData;

        public void Execute(int index)
        {
            // Move the asteroid by the random direction and speed
            AsteroidEntity asteroidEntity = asteroidsData[index];
            asteroidEntity.position += asteroidsData[index].direction * asteroidsData[index].speed * deltaTime;
            //rotate the asteroid by axis of a vector perpendicular to the direction vector and the z axis considering deltaTime
            asteroidEntity.rotation = math.mul(asteroidEntity.rotation, quaternion.AxisAngle(math.cross(asteroidEntity.direction, new float3(0, 0, 1)), asteroidEntity.speed * deltaTime * 0.1f));
            asteroidsData[index] = asteroidEntity;
        }
    }
}
