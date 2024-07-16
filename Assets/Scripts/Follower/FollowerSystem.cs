using Unity.Entities;
using Unity.Transforms;
public partial struct FollowerSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _followerEntity;
    private FollowerComponent _followerComponent;

    public void OnCreate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton(out FollowerComponent follower))
            state.EntityManager.CreateEntity(typeof(FollowerComponent));
    }
    
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;

        _followerEntity = SystemAPI.GetSingletonEntity<FollowerComponent>();
        _followerComponent = _entityManager.GetComponentData<FollowerComponent>(_followerEntity);

        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerComponent>());

        _followerComponent.TargetPosition = playerTransform.Position;

        _entityManager.SetComponentData(_followerEntity, _followerComponent);

        PlayerEntityPosition.targetPosition = _followerComponent.TargetPosition;
    }
}
