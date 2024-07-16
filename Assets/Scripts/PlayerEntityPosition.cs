using System.Collections;
using UnityEngine;

public class PlayerEntityPosition : MonoBehaviour
{
    public static Vector3 targetPosition;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    public GameObject folllowObject;

    private void Awake()
    {
        folllowObject = new GameObject("Follower");

        if (targetPosition != null)
            folllowObject.transform.position = targetPosition;
    }
    private void Start()
    {
        virtualCamera.Follow = folllowObject.transform;
        virtualCamera.LookAt = folllowObject.transform;
    }
    private void Update()
    {
        folllowObject.transform.position = targetPosition;
    }
}
