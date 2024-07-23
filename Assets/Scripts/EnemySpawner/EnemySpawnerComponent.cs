using System.Collections;
using Unity.Entities;
using UnityEngine;

public struct EnemySpawnerComponent : IComponentData
{
    public Entity EnemyPrefab;

    public int NumOfEnemiesToSpawnPerSecond;
    public int NumOfEnemiesToSpawnIncrementAmount;
    public int MaxNumOfEnemiesToSpawnPerSecond;

    public float CurrenTimeBeforeNextSpawn;
    public float TimeBeforeNextSpawn;
    public float EnemySpawnRadius;
    public float MinDistanceFromPlayer;
    public float EnemyHealth;
    public float EnemySpeed;
}
