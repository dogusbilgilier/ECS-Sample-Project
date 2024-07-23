using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    [Header("Referneces - Assets")]
    public GameObject EnemyPrefab;
    [Space()]
    [Header("Spawn Options")]
    public int NumOfEnemiesToSpawnPerSecond;
    public int NumOfEnemiesToSpawnIncrementAmount;
    public int MaxNumOfEnemiesToSpawnPerSecond;

    public float TimeBeforeNextSpawn;
    public float EnemySpawnRadius;
    public float MinDistanceFromPlayer;
    public float EnemyHealth;
    public float EnemySpeed;


    public class EnemySpawnBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(enemySpawnerEntity, new EnemySpawnerComponent()
            {
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),

                NumOfEnemiesToSpawnPerSecond = authoring.NumOfEnemiesToSpawnPerSecond,
                NumOfEnemiesToSpawnIncrementAmount = authoring.NumOfEnemiesToSpawnIncrementAmount,
                MaxNumOfEnemiesToSpawnPerSecond = authoring.MaxNumOfEnemiesToSpawnPerSecond,
                MinDistanceFromPlayer = authoring.MinDistanceFromPlayer,

                EnemySpeed = authoring.EnemySpeed,
                EnemyHealth = authoring.EnemyHealth,
                EnemySpawnRadius = authoring.EnemySpawnRadius,
                TimeBeforeNextSpawn = authoring.TimeBeforeNextSpawn
            });
        }
    }
}
