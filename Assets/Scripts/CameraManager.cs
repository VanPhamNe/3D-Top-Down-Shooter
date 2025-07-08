using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    [Header("Camera Distance")]
    private float targetCameraDistance;
    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float distanceChangeRate;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          
        }
        else
        {
            Destroy(gameObject);
        }
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void Update()
    {
        UpdateCameraDistance();

    }

    private void UpdateCameraDistance()
    {
        if (canChangeCameraDistance ==false)
        {
            return;
        }
        float currentCameraDistance = framingTransposer.m_CameraDistance;
        if (Mathf.Abs(targetCameraDistance - currentCameraDistance) > 0.01f)
        {
            framingTransposer.m_CameraDistance = Mathf.Lerp(currentCameraDistance, targetCameraDistance, distanceChangeRate * Time.deltaTime); // Lerp to the target distance smoothly
        }
    }

    public void ChangeCameraDistance(float distance)
    {
        targetCameraDistance = distance;
    }


}
