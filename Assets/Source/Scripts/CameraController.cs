using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kuhpik;
using UnityEngine;

public class CameraController : GameSystem
{
    [SerializeField] private List<CameraConfiguration> cameraConfigurations;

    public override void OnInit()
    {
        base.OnInit();
        ChangeCamera(CameraType.Walk);
    }

    public void ChangeCamera(CameraType cameraType)
    {
        foreach (var cameraConfiguration in cameraConfigurations)
        {
            cameraConfiguration.VirtualCamera.Priority = cameraType == cameraConfiguration.CameraType ? 1 : 0;
        }
    }
}

[Serializable]
public class CameraConfiguration
{
    [SerializeField] private CameraType cameraType;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public CameraType CameraType => cameraType;

    public CinemachineVirtualCamera VirtualCamera => virtualCamera;
}

public enum CameraType
{
    Walk,
    ChangePoop,
    Upgrade,
    Mother,
    Child,
    Poop,
    Bed,
    Eat
}