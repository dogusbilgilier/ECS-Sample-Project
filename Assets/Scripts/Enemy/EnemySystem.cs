using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<PlayerComponent>(out PlayerComponent playerComponent))
            return;

        NativeArray<Entity> entities = state.EntityManager.GetAllEntities();

        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = state.EntityManager.GetComponentData<LocalTransform>(playerEntity);

        foreach (Entity entity in entities)
        {
            if (state.EntityManager.HasComponent<EnemyComponent>(entity))
            {
                LocalTransform enemyTransform = state.EntityManager.GetComponentData<LocalTransform>(entity);
                EnemyComponent enemyComponent = state.EntityManager.GetComponentData<EnemyComponent>(entity);

                float3 dir = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(dir.x, dir.z);
                quaternion lookRot = quaternion.AxisAngle(new float3(0, 1, 0), angle);

                enemyTransform.Rotation = lookRot;
                enemyTransform.Position += enemyComponent.EnemySpeed * SystemAPI.Time.DeltaTime * enemyTransform.Forward();

                state.EntityManager.SetComponentData(entity, enemyTransform);
            }
        }
    }
}
