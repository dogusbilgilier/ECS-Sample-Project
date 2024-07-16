using System.Collections;
using Unity.Entities;
using UnityEngine;

public struct BulletLifetimeComponent : IComponentData
{
    public float ReaminingLifeTime;
}
