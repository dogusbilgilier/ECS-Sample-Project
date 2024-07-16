using Unity.Entities;
using Unity.Mathematics;

public struct FollowerComponent : IComponentData
{
    public float3 TargetPosition;
}
