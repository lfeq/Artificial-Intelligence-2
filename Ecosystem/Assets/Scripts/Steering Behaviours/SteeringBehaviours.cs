using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The SteeringBehaviours class provides functions for implementing steering behaviors in autonomous BaseAgents.
/// </summary>
public class SteeringBehaviours {

    #region public functions

    /// <summary>
    /// Implements the "seek" behavior to move the BaseAgent towards a specified target.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent seeking the target.</param>
    /// <param name="t_target">The position of the target.</param>
    /// <param name="useArrival">Flag indicating whether to use arrival behavior.</param>
    /// <returns>The steering force vector for the seek behavior.</returns>
    public static Vector3 seek(BaseAgent t_BaseAgent, Vector3 t_target, bool useArrival = false) {
        Vector3 desiredVelocity = t_target - t_BaseAgent.transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= t_BaseAgent.maxSpeed;
        if (useArrival) {
            desiredVelocity = arrival(t_BaseAgent, t_target, desiredVelocity);
        }
        return calculateSteeringForce(t_BaseAgent, desiredVelocity);
    }

    /// <summary>
    /// Implements the "flee" behavior to move the BaseAgent away from a specified target position.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent fleeing from the target position.</param>
    /// <param name="t_targetPosition">The position of the target to flee from.</param>
    /// <returns>The steering force vector for the flee behavior.</returns>
    public static Vector3 flee(BaseAgent t_BaseAgent, Vector3 t_targetPosition) {
        Vector3 desiredVelocity = t_BaseAgent.transform.position - t_targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= t_BaseAgent.maxSpeed;
        return calculateSteeringForce(t_BaseAgent, desiredVelocity);
    }

    /// <summary>
    /// Implements the "pursuit" behavior to make the BaseAgent pursue a target BaseAgent's predicted future position.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent initiating the pursuit.</param>
    /// <param name="t_BaseAgentToPursuit">The target BaseAgent to pursue.</param>
    /// <returns>The steering force vector for the pursuit behavior.</returns>
    public static Vector3 pursuit(BaseAgent t_BaseAgent, BaseAgent t_BaseAgentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_BaseAgentToPursuit.transform.position, t_BaseAgent.transform.position);
        float positionPrediction = distanceToTarget / t_BaseAgent.maxSpeed;
        Vector3 futurePosition = t_BaseAgentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_BaseAgentToPursuit.transform.position;
        return seek(t_BaseAgent, futurePosition);
    }

    /// <summary>
    /// Implements the "evade" behavior to make the BaseAgent move away from the predicted future position of a target BaseAgent.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent evading the target.</param>
    /// <param name="t_BaseAgentToPursuit">The target BaseAgent from which to evade.</param>
    /// <returns>The steering force vector for the evade behavior.</returns>
    public static Vector3 evade(BaseAgent t_BaseAgent, BaseAgent t_BaseAgentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_BaseAgentToPursuit.transform.position, t_BaseAgent.transform.position);
        float positionPrediction = distanceToTarget / t_BaseAgent.maxSpeed;
        Vector3 futurePosition = t_BaseAgentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_BaseAgentToPursuit.transform.position;
        return flee(t_BaseAgent, futurePosition);
    }

    /// <summary>
    /// Implements the "wander" behavior to create a random, meandering motion for the BaseAgent.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent applying the wander behavior.</param>
    /// <returns>The steering force vector for the wander behavior.</returns>
    private static Vector3 circleCenter;

    public static Vector3 wander(BaseAgent t_BaseAgent) {
        circleCenter = t_BaseAgent.getCurrentVelocity();
        circleCenter.Normalize();
        circleCenter *= t_BaseAgent.circleDistance;
        Vector3 displacement = new Vector3(0, 0, -1);
        displacement *= t_BaseAgent.circleRadius;
        float displacementMagnitud = displacement.magnitude;
        float angleChange = t_BaseAgent.angleChange;
        displacement.x = Mathf.Cos(t_BaseAgent.wanderAngle) * displacementMagnitud;
        displacement.z = Mathf.Sin(t_BaseAgent.wanderAngle) * displacementMagnitud;
        t_BaseAgent.wanderAngle += (Random.value * angleChange) - (angleChange * 0.5f);
        Vector3 wanderForce = circleCenter + displacement;
        wanderForce = Vector3.ClampMagnitude(wanderForce, t_BaseAgent.maxSpeed);
        wanderForce /= t_BaseAgent.getMass();
        return Vector3.ClampMagnitude((t_BaseAgent.getCurrentVelocity() + wanderForce), t_BaseAgent.maxSpeed);
    }

    public static Vector3 collisionAvoidance(BaseAgent t_BaseAgent) {
        Vector3 agentPosition = t_BaseAgent.transform.position;
        float dynamicLenght = t_BaseAgent.getCurrentVelocity().magnitude / t_BaseAgent.maxSpeed;
        Vector3 ahead = agentPosition + (t_BaseAgent.getCurrentVelocity().normalized * dynamicLenght);
        Vector3 ahead2 = agentPosition + (t_BaseAgent.getCurrentVelocity().normalized * dynamicLenght * 0.5f);
        Vector3 avoidance = Vector3.zero;
        DebugExtension.DebugArrow(agentPosition, ahead, Color.green);
        DebugExtension.DebugArrow(agentPosition, ahead2, Color.red);
        GameObject mostThreateningObstacle = findMostThreateningObstacle(ahead, ahead2, t_BaseAgent.collisionObstacleAvoidanceRadius, agentPosition);
        if (mostThreateningObstacle == null) {
            return avoidance;
        }
        avoidance.x = ahead.x - mostThreateningObstacle.transform.position.x;
        avoidance.z = ahead.z - mostThreateningObstacle.transform.position.z;
        avoidance.Normalize();
        avoidance *= t_BaseAgent.collisionAvoidanceForce;
        DebugExtension.DebugArrow(agentPosition, avoidance, Color.yellow);
        return avoidance;
    }

    #endregion public functions

    #region private functions

    /// <summary>
    /// Implements the "arrival" behavior to slow down the BaseAgent as it approaches the target.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent applying the arrival behavior.</param>
    /// <param name="target">The target position.</param>
    /// <param name="desiredVelocity">The desired velocity vector before arrival adjustments.</param>
    /// <returns>The adjusted steering force vector for the arrival behavior.</returns>
    private static Vector3 arrival(BaseAgent t_BaseAgent, Vector3 target, Vector3 desiredVelocity) {
        float distance = Vector3.Distance(t_BaseAgent.transform.position, target);
        if (distance <= t_BaseAgent.slowingRadius) {
            desiredVelocity.Normalize();
            desiredVelocity *= t_BaseAgent.maxSpeed * (distance / t_BaseAgent.slowingRadius);
        }
        return desiredVelocity;
    }

    /// <summary>
    /// Calculates the steering force to be applied to the BaseAgent based on the desired velocity.
    /// </summary>
    /// <param name="t_BaseAgent">The BaseAgent for which the steering force is calculated.</param>
    /// <param name="t_desiredVelocity">The desired velocity vector.</param>
    /// <returns>The steering force vector for the BaseAgent.</returns>
    private static Vector3 calculateSteeringForce(BaseAgent t_BaseAgent, Vector3 t_desiredVelocity) {
        Vector3 steeringForce = t_desiredVelocity - t_BaseAgent.getCurrentVelocity();
        steeringForce = Vector3.ClampMagnitude(steeringForce, t_BaseAgent.maxSteeringForce);
        steeringForce /= t_BaseAgent.getMass(); ;
        return Vector3.ClampMagnitude((t_BaseAgent.getCurrentVelocity() + steeringForce), t_BaseAgent.maxSpeed);
    }

    private static GameObject findMostThreateningObstacle(Vector3 t_ahead, Vector3 t_ahead2, float t_radius, Vector3 t_agentPosition) {
        GameObject mostThreateningObstacle = null;
        Collider[] hitColliders = Physics.OverlapSphere(t_ahead, t_radius);
        float maxDistance = float.MaxValue;
        foreach (var hitCollider in hitColliders) {
            float distanceToObstacle = Vector3.Distance(t_agentPosition, hitCollider.transform.position);
            if (distanceToObstacle < maxDistance) {
                maxDistance = distanceToObstacle;
                mostThreateningObstacle = hitCollider.gameObject;
            }
        }
        hitColliders = Physics.OverlapSphere(t_ahead2, t_radius);
        foreach (var hitCollider in hitColliders) {
            float distanceToObstacle = Vector3.Distance(t_agentPosition, hitCollider.transform.position);
            if (distanceToObstacle < maxDistance) {
                maxDistance = distanceToObstacle;
                mostThreateningObstacle = hitCollider.gameObject;
            }
        }
        return mostThreateningObstacle;
    }

    #endregion private functions
}