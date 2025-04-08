using UnityEngine;
//using Cinemachine;
using Unity.Cinemachine;

public class CameraRotationControl : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust as needed
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachinePOV _cinemachinePOV;

    void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachinePOV = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _cinemachinePOV.m_HorizontalAxis.Value += rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _cinemachinePOV.m_HorizontalAxis.Value -= rotationSpeed * Time.deltaTime;
        }
    }
}