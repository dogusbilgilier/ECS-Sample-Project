using System.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float SpawnBulletInterval;
    public GameObject BulletPrefab;

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(playerEntity, new PlayerComponent()
            {
                MoveSpeed = authoring.MoveSpeed,
                SpawnBulletInterval = authoring.SpawnBulletInterval,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic)
            });

        }
    }
}
