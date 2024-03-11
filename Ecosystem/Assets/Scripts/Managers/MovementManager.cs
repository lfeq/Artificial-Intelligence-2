using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BaseAgent))]
public class MovementManager : MonoBehaviour {
    private Rigidbody rb;
    private MovementState movementState;
    private BaseAgent agent;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        movementState = MovementState.None;
        agent = GetComponent<BaseAgent>();
    }

    private void FixedUpdate() {
        move();
    }

    public void setMovementState(MovementState t_movementState) {
        if (movementState == t_movementState) {
            return;
        }
        movementState = t_movementState;
    }

    private void move() {
        Vector3 steeringForce = Vector3.zero;
        switch (movementState) {
            case MovementState.None:
                break;
            case MovementState.Pursuing:
                break;
            case MovementState.Evading:
                break;
            case MovementState.Arriving:
                steeringForce += SteeringBehaviours.seek(agent, agent.target.position, false);
                //steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
            case MovementState.Wandering:
                steeringForce += SteeringBehaviours.wander(agent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
        }
        rb.velocity = steeringForce;
    }
}

public enum MovementState {
    None,
    Pursuing,
    Evading,
    Arriving,
    Wandering
}