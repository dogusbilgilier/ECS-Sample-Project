using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (var entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifetimeComponent>(entity))
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTransform.Position += bulletComponent.TravelSpeed * SystemAPI.Time.DeltaTime * bulletTransform.Forward();
                entityManager.SetComponentData(entity, bulletTransform);

                BulletLifetimeComponent bulletLifetimeComponent = entityManager.GetComponentData<BulletLifetimeComponent>(entity);
                bulletLifetimeComponent.ReaminingLifeTime -= SystemAPI.Time.DeltaTime;

                if (bulletLifetimeComponent.ReaminingLifeTime < 0)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }
                entityManager.SetComponentData(entity, bulletLifetimeComponent);

                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                physicsWorld.SphereCastAll(bulletTransform.Position, 0.5f, float3.zero, 0f, ref hits, new CollisionFilter
                {
                    BelongsTo = (uint)CollisionLayer.Default,
                    CollidesWith = (uint)CollisionLayer.Enemy
                });

                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        Entity hitEntity = hits[i].Entity;

                        if (entityManager.HasComponent<EnemyComponent>(hitEntity))
                        {
                            EnemyComponent enemyComponent = entityManager.GetComponentData<EnemyComponent>(hitEntity);
                            enemyComponent.CurrentHealt -= bulletComponent.Damage;

                            entityManager.SetComponentData(hitEntity, enemyComponent);
                            entityManager.SetComponentData(entity, bulletComponent);

                            if (enemyComponent.CurrentHealt <= 0)
                                entityManager.DestroyEntity(hitEntity);
                        }
                    }
                    entityManager.DestroyEntity(entity);
                }
                hits.Dispose();
            }
        }
    }
}
