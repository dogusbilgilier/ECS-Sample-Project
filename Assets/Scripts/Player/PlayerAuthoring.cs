using System.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float MoveSpeed;
    public float SpawnBulletInterval;

    public float BulletSpeed;
    public float BulletLifeTime;
    public float BulletDamage;


    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(playerEntity, new PlayerComponent()
            {
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                MoveSpeed = authoring.MoveSpeed,
                SpawnBulletInterval = authoring.SpawnBulletInterval,
                BulletDamage = authoring.BulletDamage,
                BulletLifetime = authoring.BulletLifeTime,
                BulletSpeed = authoring.BulletSpeed
            });

        }
    }
}
