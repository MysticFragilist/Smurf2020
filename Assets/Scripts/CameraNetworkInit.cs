using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class CameraNetworkInit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    }

    public void setTarget(Transform target)
    {
        this.GetComponentsInChildren<CinemachineVirtualCamera>()[0].Follow = target;
    }
}
