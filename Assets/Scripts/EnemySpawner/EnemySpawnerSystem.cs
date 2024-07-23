using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial struct EnemySpawnerSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _enemySpawnerEntity;
    private EnemySpawnerComponent _enemySpawnerComponent;
    private Entity _playerEntity;

    private Unity.Mathematics.Random _random;

    public void OnCreate(ref SystemState state)
    {
        _random = Unity.Mathematics.Random.CreateFromIndex((uint)_enemySpawnerComponent.GetHashCode());
    }
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
        _enemySpawnerComponent = _entityManager.GetComponentData<EnemySpawnerComponent>(_enemySpawnerEntity);
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        SpawnEnemies(ref state);
    }
    private void SpawnEnemies(ref SystemState state)
    {
        _enemySpawnerComponent.CurrenTimeBeforeNextSpawn -= SystemAPI.Time.DeltaTime;

        if (_enemySpawnerComponent.CurrenTimeBeforeNextSpawn <= 0)
        {
            for (int i = 0; i < _enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond; i++)
            {
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
                Entity enemyEntity = _entityManager.Instantiate(_enemySpawnerComponent.EnemyPrefab);

                LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(enemyEntity);
                LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

                float minDistanceSquared = _enemySpawnerComponent.MinDistanceFromPlayer * _enemySpawnerComponent.MinDistanceFromPlayer;
                float2 randomOffset = _random.NextFloat2Direction() * _random.NextFloat2(_enemySpawnerComponent.MinDistanceFromPlayer, _enemySpawnerComponent.EnemySpawnRadius);
                float2 playerPosition = new float2(playerTransform.Position.x, playerTransform.Position.z);
                float2 spawnPosition = playerPosition + randomOffset;
                float distanceSquared = math.lengthsq(spawnPosition - playerPosition);

                if (distanceSquared < minDistanceSquared)
                {
                    spawnPosition = playerPosition + math.normalize(randomOffset) * math.sqrt(minDistanceSquared);
                }

                enemyTransform.Position = new float3(spawnPosition.x, 1f, spawnPosition.y);


                float3 dir = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(dir.x, dir.z);
                quaternion lookRot = quaternion.AxisAngle(new float3(0, 1, 0), angle);
                enemyTransform.Rotation = lookRot;

                ecb.SetComponent(enemyEntity, enemyTransform);

                ecb.AddComponent(enemyEntity, new EnemyComponent()
                {
                    CurrentHealt = _enemySpawnerComponent.EnemyHealth,
                    EnemySpeed = _enemySpawnerComponent.EnemySpeed
                });

                ecb.Playback(_entityManager);
                ecb.Dispose();

            }
                int desiredEnemiesPerWave = _enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond + _enemySpawnerComponent.NumOfEnemiesToSpawnIncrementAmount;
                int enemiesPerWave = math.min(desiredEnemiesPerWave, _enemySpawnerComponent.MaxNumOfEnemiesToSpawnPerSecond);

                _enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond = enemiesPerWave;
                _enemySpawnerComponent.CurrenTimeBeforeNextSpawn = _enemySpawnerComponent.TimeBeforeNextSpawn;
        }
        _entityManager.SetComponentData(_enemySpawnerEntity, _enemySpawnerComponent);
    }
}
