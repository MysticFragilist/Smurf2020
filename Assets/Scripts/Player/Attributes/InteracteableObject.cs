using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class InteracteableObject : NetworkBehaviour
{
    public List<ActionableObject> listActionableObjects;

    bool isActive = false;

    [Header("Events")]
	[Space]
	public UnityEvent OnActiveEvent;

    private void Awake()
	{
        if (OnActiveEvent == null)
			OnActiveEvent = new UnityEvent();
    }

    public void CheckIfActive() {
        foreach (ActionableObject obj in listActionableObjects) {
            if (!obj.isActivated) return;
        }
        OnActiveEvent.Invoke();
    }
}
