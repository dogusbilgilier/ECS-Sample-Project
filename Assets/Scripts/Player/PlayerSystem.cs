using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Burst;
[BurstCompile]
public partial struct PlayerSystem : ISystem
{

    private EntityManager _entityManager;

    private Entity _playerEntity;
    private Entity _inputEntity;

    private PlayerComponent _playerComponent;
    private InputComponent _inputComponent;

    private float lastShootTime;
    public void OnCreate(ref SystemState state)
    {
        lastShootTime = (float)SystemAPI.Time.ElapsedTime;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        _inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        _playerComponent = _entityManager.GetComponentData<PlayerComponent>(_playerEntity);
        _inputComponent = _entityManager.GetComponentData<InputComponent>(_inputEntity);

        Move(ref state);

        if ((float)SystemAPI.Time.ElapsedTime >= lastShootTime + _playerComponent.SpawnBulletInterval)
            Shoot(ref state);
    }

    [BurstCompile]
    private void Shoot(ref SystemState state)
    {
        if (_inputComponent.Shoot)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity bulletEntity = _entityManager.Instantiate(_playerComponent.BulletPrefab);

            ecb.AddComponent(bulletEntity, new BulletComponent()
            {
                TravelSpeed = _playerComponent.BulletSpeed,
                Damage = _playerComponent.BulletDamage
            });
            ecb.AddComponent(bulletEntity, new BulletLifetimeComponent() { ReaminingLifeTime = _playerComponent.BulletLifetime });

            LocalTransform bulletTransform = _entityManager.GetComponentData<LocalTransform>(bulletEntity);
            LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

            bulletTransform.Position = playerTransform.Position;
            bulletTransform.Rotation = playerTransform.Rotation;

            ecb.SetComponent(bulletEntity, bulletTransform);
            ecb.Playback(_entityManager);

            ecb.Dispose();

            lastShootTime = (float)SystemAPI.Time.ElapsedTime;
        }
    }

    [BurstCompile]
    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);
        playerTransform.Position += new float3(_inputComponent.Movement.x, 0f, _inputComponent.Movement.y) * _playerComponent.MoveSpeed * SystemAPI.Time.DeltaTime;

        Vector2 dir = (Vector2)_inputComponent.MousePosition - (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.x, dir.y));
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.up);
        _entityManager.SetComponentData(_playerEntity, playerTransform);
    }
}
