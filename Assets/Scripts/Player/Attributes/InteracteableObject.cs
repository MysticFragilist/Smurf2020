using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class InteracteableObject : NetworkBehaviour
{
    public List<ActionableObject> listActionableObjects;
    public bool andGate = true;

    public bool isActive { get; private set; } = false;

    [Header("Events")]
	[Space]
	public UnityEvent OnActiveEvent;

    public override void OnStartClient() {
        Debug.Log("Spawn in server");
    }
    private void Awake()
	{
        if (OnActiveEvent == null)
			OnActiveEvent = new UnityEvent();
    }

    public void CheckIfActive() {
        CmdCheckAlive();
    }

    [Command]
    public void CmdCheckAlive() {
        if (andGate) {
            foreach (ActionableObject obj in listActionableObjects) {
                if (!obj.isActivated)  {
                    RpcSendState(false);
                    return;
                }
            }
            
            isActive = true;
            OnActiveEvent.Invoke();
        }
        else {
            foreach (ActionableObject obj in listActionableObjects) {
                if (obj.isActivated)  {
                    RpcSendState(true);
                    return;
                }
            }

            RpcSendState(false);
            OnActiveEvent.Invoke();
        }
    }

    [ClientRpc]
    public void RpcSendState(bool newActive) {
        isActive = newActive;
    }
}
