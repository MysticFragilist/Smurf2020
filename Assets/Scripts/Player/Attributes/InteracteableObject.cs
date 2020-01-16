using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class InteracteableObject : NetworkBehaviour
{
    public List<ActionableObject> listActionableObjects;

    public bool isActive { get; private set; } = false;

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
            if (!obj.isActivated)  {
                isActive = false;
                return;
            }
        }
        isActive = true;
        OnActiveEvent.Invoke();
    }
}
