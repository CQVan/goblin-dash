using System.Drawing;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Transform player;

    [Header("Camera Controls")]
    [Range(1, 8)]
    [SerializeField] private int steps = 4;
    [Range(0,1)]
    [SerializeField] private float rotSpeed;

    private CinemachineFollow cinemachineFollow;
    private Vector3 baseOffset;
    private float targetAngle = 0;
    private float currentAngle = 0;

    private void Awake()
    {
        cinemachineFollow = GetComponent<CinemachineFollow>();
        baseOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Rot Right
            HandleCameraRotation(-1);
        if (Input.GetKeyDown(KeyCode.Q))
            HandleCameraRotation(1);


        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotSpeed);
        cinemachineFollow.FollowOffset = Quaternion.AngleAxis(currentAngle, Vector3.up) * baseOffset;
    }

    private void HandleCameraRotation(int direction)
    {
        float rotationAmount = 360 / steps * direction;

        targetAngle += rotationAmount;
    }
}
