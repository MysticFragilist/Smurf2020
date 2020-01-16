using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum StateElevator {
    UP,
    MOVING_UP,
    MOVING_DOWN,
    DOWN
}
public class ElevatorObject : NetworkBehaviour
{
    public Transform positionToGo;
    Vector3 elevatorOrigin;

    public float speed = 3f;
    [SyncVar(hook="OnStateChange")]
    private StateElevator m_state;
    public StateElevator State { get { return m_state;} }

    void Start() {
        m_state = StateElevator.DOWN;
        elevatorOrigin = this.transform.position;
        Debug.Log("Origin:" + elevatorOrigin.y);
    }
    private void FixedUpdate() {
        if(m_state == StateElevator.MOVING_UP && this.transform.position.y >= positionToGo.position.y) {
            m_state = StateElevator.UP;
        }
        else if(m_state == StateElevator.MOVING_DOWN && this.transform.position.y <= this.elevatorOrigin.y) {
            Debug.Log("arrived");
            Debug.Log("origin: " + this.elevatorOrigin.y + " actual:" + transform.position.y);
            m_state = StateElevator.DOWN;
        }

        if(m_state == StateElevator.MOVING_UP) {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else if(m_state == StateElevator.MOVING_DOWN) {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
            Debug.Log("moving down");
        }
    }
    public void OnActiveChange()
    {
        if(m_state == StateElevator.DOWN) {
            m_state = StateElevator.MOVING_UP;
        }
        else if (m_state == StateElevator.UP) {
            m_state = StateElevator.MOVING_DOWN;
        }
    }
}
