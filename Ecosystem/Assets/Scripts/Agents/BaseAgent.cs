using UnityEngine;

//TODO: Add Reproduction
[RequireComponent(typeof(Rigidbody))]
public class BaseAgent : MonoBehaviour {
    [SerializeField] private BaseAgentData agentData;

    private Rigidbody m_rb;

    #region Unity functions

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyePosition.position, eyeRadius);
    }

    #endregion Unity functions

    public void init(BaseAgentData t_baseAgent) {
        genre = t_baseAgent.genre;
        maxSpeed = t_baseAgent.maxSpeed;
        maxSteeringForce = t_baseAgent.maxSteeringForce;
        slowingRadius = t_baseAgent.slowingRadius;
        eyeRadius = t_baseAgent.eyeRadius;
        collisionObstacleAvoidanceRadius = t_baseAgent.collisionObstacleAvoidanceRadius;
        collisionAvoidanceForce = t_baseAgent.collisionAvoidanceForce;
        maxHunger = t_baseAgent.maxHunger;
        hungerTreshold = t_baseAgent.hungerTreshold;
        hungerRatePerSecond = t_baseAgent.hungerRatePerSecond;
        maxThirst = t_baseAgent.maxThirst;
        thirstTreshold = t_baseAgent.thirstTreshold;
        thirstRatePerSecond = t_baseAgent.thirstRatePerSecond;
        attractiveness = t_baseAgent.attractiveness;
    }

    public Vector3 getCurrentVelocity() {
        return m_rb.velocity;
    }

    public float getMass() {
        return m_rb.mass;
    }

    public float wanderAngle {
        get { return agentData.wanderAngle; }
        set { agentData.wanderAngle = value; }
    }

    public Genre genre {
        get { return agentData.genre; }
        set { agentData.genre = value; }
    }

    public float maxSpeed {
        get { return agentData.maxSpeed; }
        set { agentData.maxSpeed = value; }
    }

    public float maxSteeringForce {
        get { return agentData.maxSteeringForce; }
        set { agentData.maxSteeringForce = value; }
    }

    public float slowingRadius {
        get { return agentData.slowingRadius; }
        set { agentData.slowingRadius = value; }
    }

    public Transform target {
        get { return agentData.target; }
        set { agentData.target = value; }
    }

    public float eyeRadius {
        get { return agentData.eyeRadius; }
        set { agentData.eyeRadius = value; }
    }

    public Transform eyePosition {
        get { return agentData.eyePosition; }
        set { agentData.eyePosition = value; }
    }

    public float angleChange {
        get { return agentData.angleChange; }
        set { agentData.angleChange = value; }
    }

    public float circleDistance {
        get { return agentData.circleDistance; }
        set { agentData.circleDistance = value; }
    }

    public float circleRadius {
        get { return agentData.circleRadius; }
        set { agentData.circleRadius = value; }
    }

    public float collisionObstacleAvoidanceRadius {
        get { return agentData.collisionObstacleAvoidanceRadius; }
        set { agentData.collisionObstacleAvoidanceRadius = value; }
    }

    public float collisionAvoidanceForce {
        get { return agentData.collisionAvoidanceForce; }
        set { agentData.collisionAvoidanceForce = value; }
    }

    public float maxHunger {
        get { return agentData.maxHunger; }
        set { agentData.maxHunger = value; }
    }

    public float hungerTreshold {
        get { return agentData.hungerTreshold; }
        set { agentData.hungerTreshold = value; }
    }

    public float hungerRatePerSecond {
        get { return agentData.hungerRatePerSecond; }
        set { agentData.hungerRatePerSecond = value; }
    }

    public float eatDistance {
        get { return agentData.eatDistance; }
        set { agentData.eatDistance = value; }
    }

    public float maxThirst {
        get { return agentData.maxThirst; }
        set { agentData.maxThirst = value; }
    }

    public float thirstTreshold {
        get { return agentData.thirstTreshold; }
        set { agentData.thirstTreshold = value; }
    }

    public float thirstRatePerSecond {
        get { return agentData.thirstRatePerSecond; }
        set { agentData.thirstRatePerSecond = value; }
    }

    public float drinkingDistance {
        get { return agentData.drinkingDistance; }
        set { agentData.drinkingDistance = value; }
    }

    public float gestationTimeInSeconds {
        get { return agentData.gestationTimeInSeconds; }
        set { agentData.gestationTimeInSeconds = value; }
    }

    public float reproductionDistance {
        get { return agentData.reproductionDistance; }
        set { agentData.reproductionDistance = value; }
    }

    public bool isPregnant {
        get { return agentData.isPregnant; }
        set {
            if (genre == Genre.Male) {
                return;
            }
            agentData.isPregnant = value;
        }
    }

    public float attractiveness {
        get { return agentData.attractiveness; }
        set { agentData.attractiveness = value; }
    }

    public float reproductionTreshold {
        get { return agentData.reproductionTreshold; }
        set { agentData.reproductionTreshold = value; }
    }

    public int maxBabies {
        get { return agentData.maxBabies; }
        set { agentData.maxBabies = value; }
    }

    public int minBabies {
        get { return agentData.minBabies; }
        set { agentData.minBabies = value; }
    }
}

public enum Genre {
    Female,
    Male
}