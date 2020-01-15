using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class CameraNetworkInit : MonoBehaviour
{
    public void setTarget(Transform target)
    {
        this.GetComponentsInChildren<CinemachineVirtualCamera>()[0].Follow = target;
        CinemachineFramingTransposer framingBody = this.GetComponentsInChildren<CinemachineVirtualCamera>()[0].GetCinemachineComponent<CinemachineFramingTransposer>();
        
        framingBody.m_DeadZoneHeight = 1;
    }
}
