// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
// namespace DefaultNamespace
// {
//     public class SpaceshipEnemySystem : ComponentSystem
//     {
//         private Random random;
//         float timeSinceLastShot = 0f;
//         protected override void OnCreate()
//         {
//             base.OnCreate();
//             random = new Random(8979546);
//         }
//         protected override void OnUpdate()
//         {
//             //check if the spaceship singleton exists
//             if (!HasSingleton<SpaceshipEnemyEntity>())
//             {
//                 GamePrefabsSingleton prefabContainer = GetSingleton<GamePrefabsSingleton>();
//                 Entity spaceshipEnemyPrefab = prefabContainer.spaceshipEnemyPrefab;
//                 Entity sEntity = EntityManager.Instantiate(spaceshipEnemyPrefab);
//                 SpaceshipEnemyEntity spaceshipEnemy = EntityManager.GetComponentData<SpaceshipEnemyEntity>(sEntity);
//                 float3 randomPosition = new float3(random.NextFloat(150f, 500f), random.NextFloat(150f, 500f), 0f);
//                 spaceshipEnemy.position = randomPosition;
//
//                 if (random.NextInt(0, 2) == 0)
//                 {
//                     spaceshipEnemy.position.x *= -1;
//                 }
//
//                 if (random.NextInt(0, 2) == 0)
//                 {
//                     spaceshipEnemy.position.y *= -1;
//                 }
//
//                 float3 randomDirection = math.normalize(new float3(random.NextFloat(-1f, 1f), random.NextFloat(-1f, 1f), 0f));
//                 spaceshipEnemy.direction = randomDirection;
//                 EntityManager.SetComponentData(sEntity, spaceshipEnemy);
//             }
//
//             Entity spaceshipEnemyEntity = GetSingletonEntity<SpaceshipEnemyEntity>();
//             SpaceshipEnemyEntity enemy = EntityManager.GetComponentData<SpaceshipEnemyEntity>(spaceshipEnemyEntity);
//             Translation translation = EntityManager.GetComponentData<Translation>(spaceshipEnemyEntity);
//             enemy.position += enemy.direction * 10f * Time.DeltaTime;
//             translation.Value = enemy.position;
//             EntityManager.SetComponentData(spaceshipEnemyEntity, translation);
//             EntityManager.SetComponentData(spaceshipEnemyEntity, enemy);
//
//
//             Entities
//                 .WithAll<OldBulletEntity>()
//                 .ForEach((Entity bulletEntity, ref OldBulletEntity bullet) =>
//                 {
//                     //check if the bullet is close to the spaceship
//                     if (math.length(bullet.position - enemy.position) < 20f)
//                     {
//                         //destroy the bullet
//                         PostUpdateCommands.DestroyEntity(bulletEntity);
//                         //destroy the spaceship
//                         PostUpdateCommands.DestroyEntity(spaceshipEnemyEntity);
//                     }
//                 });
//
//             if (timeSinceLastShot > 2f)
//             {
//                 if (HasSingleton<SpaceshipEntity>())
//                 {
//                     SpaceshipEntity spaceship = GetSingleton<SpaceshipEntity>();
//                     Entity bulletEnemyPrefab = GetSingleton<GamePrefabsSingleton>().bulletEnemyPrefab;
//                     Entity bulletEnemyEntity = EntityManager.Instantiate(bulletEnemyPrefab);
//                     BulletEnemyEntity bulletEnemy = EntityManager.GetComponentData<BulletEnemyEntity>(bulletEnemyEntity);
//                     bulletEnemy.position = enemy.position;
//                     bulletEnemy.speed = 100f;
//                     bulletEnemy.direction = math.normalize(spaceship.position - enemy.position);
//                     EntityManager.SetComponentData(bulletEnemyEntity, bulletEnemy);
//                 }
//                 timeSinceLastShot = 0f;
//             }
//             else
//             {
//                 timeSinceLastShot += Time.DeltaTime;
//             }
//
//
//             if (enemy.position.x > 700 || enemy.position.x < -700 || enemy.position.y > 700 || enemy.position.y < -700)
//             {
//                 PostUpdateCommands.DestroyEntity(spaceshipEnemyEntity);
//             }
//         }
//     }
// }
