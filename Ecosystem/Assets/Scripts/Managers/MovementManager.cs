using UnityEngine;

/// <summary>
/// Manages the movement behavior of an agent based on its current state.
/// </summary>
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

    /// <summary>
    /// Sets the movement state of the agent.
    /// </summary>
    /// <param name="t_movementState">The new movement state to set.</param>
    public void setMovementState(MovementState t_movementState) {
        if (movementState == t_movementState) {
            return;
        }
        movementState = t_movementState;
    }

    /// <summary>
    /// Performs movement calculations and applies steering forces to the agent's Rigidbody.
    /// </summary>
    private void move() {
        Vector3 steeringForce = Vector3.zero;
        switch (movementState) {
            case MovementState.None:
                break;
            case MovementState.Pursuing:
                if (agent.targetAgent == null) {
                    return;
                }
                steeringForce += SteeringBehaviours.pursuit(agent, agent.targetAgent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
            case MovementState.Evading:
                if (agent.targetAgent == null) {
                    return;
                }
                steeringForce += SteeringBehaviours.evade(agent, agent.targetAgent);
                steeringForce += SteeringBehaviours.collisionAvoidance(agent);
                break;
            case MovementState.Arriving:
                if (agent.target == null) {
                    return;
                }
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

    /// <summary>
    /// Adjusts the rotation of the agent to face its direction of movement.
    /// </summary>
    private void lookAtDirection() {
        Quaternion desiredRotation = Quaternion.LookRotation(rb.velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);
    }
}

/// <summary>
/// Enumeration representing the possible movement states of an agent.
/// </summary>
public enum MovementState {
    None,
    Pursuing,
    Evading,
    Arriving,
    Wandering
}