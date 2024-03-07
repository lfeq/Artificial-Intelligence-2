using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseAgent : MonoBehaviour {

    #region Public variables

    [HideInInspector] public float wanderAngle = 0.5f;

    #endregion Public variables

    #region Serializable variables

    [SerializeField, Header("Movement")] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;
    [SerializeField] private float slowingRadius = 5;
    [SerializeField] private Transform target;
    [SerializeField, Header("Sight")] private float eyeRadius;
    [SerializeField] private Transform eyePosition;
    [SerializeField, Header("Wander")] private float angleChange = 5;
    [SerializeField] private float circleDistance = 5, circleRadius = 1;
    [SerializeField, Header("Collision Avoidance")] private float collisionObstacleAvoidanceRadius = 5;
    [SerializeField] private float collisionAvoidanceForce = 5;

    #endregion Serializable variables

    #region private variables

    private Rigidbody m_rb;

    #endregion private variables

    #region Unity functions

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyePosition.position, eyeRadius);
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Gets the transform of the target for the agent.
    /// </summary>
    /// <returns>The transform of the target.</returns>
    public Transform getTargetTranform() {
        return target;
    }

    /// <summary>
    /// Sets the target transform for the agent.
    /// </summary>
    /// <param name="t_target">The new target transform.</param>
    public void setTargetTransform(Transform t_target) {
        target = t_target;
    }

    /// <summary>
    /// Gets the mass of the agent.
    /// </summary>
    /// <returns>The mass of the agent.</returns>
    public float getMass() {
        return m_rb.mass;
    }

    /// <summary>
    /// Gets the maximum speed of the agent.
    /// </summary>
    /// <returns>The maximum speed of the agent.</returns>
    public float getMaxSpeed() {
        return maxSpeed;
    }

    /// <summary>
    /// Gets the current velocity vector of the agent.
    /// </summary>
    /// <returns>The current velocity vector of the agent.</returns>
    public Vector3 getCurrentVelocity() {
        return m_rb.velocity;
    }

    /// <summary>
    /// Gets the maximum force that can be applied to the agent.
    /// </summary>
    /// <returns>The maximum force of the agent.</returns>
    public float getMaxForce() {
        return maxForce;
    }

    /// <summary>
    /// Gets the radius of the agent's visual perception represented by its eyes.
    /// </summary>
    /// <returns>The radius of the agent's visual perception.</returns>
    public float getEyeRadius() {
        return eyeRadius;
    }

    /// <summary>
    /// Gets the position of the agent's eyes.
    /// </summary>
    /// <returns>The position of the agent's eyes.</returns>
    public Vector3 getEyePosition() {
        return eyePosition.position;
    }

    /// <summary>
    /// Gets the angle change used in the wander behavior.
    /// </summary>
    /// <returns>The angle change used in the wander behavior.</returns>
    public float getAngleChange() {
        return angleChange;
    }

    public float getSlowingRadius() {
        return slowingRadius;
    }

    public float getCircleDistance() {
        return circleDistance;
    }

    public float getCircleRadius() {
        return circleRadius;
    }

    public float getCollisionObstacleAvoidanceRadius() {
        return collisionObstacleAvoidanceRadius;
    }

    public float getCollisionAvoidanceForce() {
        return collisionAvoidanceForce;
    }

    #endregion Public functions
}