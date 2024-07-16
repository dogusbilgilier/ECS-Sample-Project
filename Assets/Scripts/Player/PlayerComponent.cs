using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerComponent : IComponentData
{
    public float MoveSpeed;
    public float SpawnBulletInterval;
    public float BulletSpeed;
    public float BulletLifetime;
    public float BulletDamage;
    public Entity BulletPrefab;
}
