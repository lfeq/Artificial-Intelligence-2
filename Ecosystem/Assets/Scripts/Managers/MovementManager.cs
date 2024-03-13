using System.Threading;
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
        lookAtDirection();
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
                steeringForce += SteeringBehaviours.pursuit(agent, agent.targetAgent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
            case MovementState.Evading:
                steeringForce += SteeringBehaviours.evade(agent, agent.targetAgent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
            case MovementState.Arriving:
                steeringForce += SteeringBehaviours.seek(agent, agent.target.position, false);
                break;
            case MovementState.Wandering:
                steeringForce += SteeringBehaviours.wander(agent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
        }
        steeringForce = Vector3.ClampMagnitude(steeringForce, agent.maxSteeringForce);
        steeringForce = steeringForce / agent.getMass();
        rb.velocity = Vector3.ClampMagnitude(rb.velocity + steeringForce, agent.maxSpeed);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    private void lookAtDirection() {
        Quaternion desiredRotation = Quaternion.LookRotation(rb.velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);
    }
}

public enum MovementState {
    None,
    Pursuing,
    Evading,
    Arriving,
    Wandering
}