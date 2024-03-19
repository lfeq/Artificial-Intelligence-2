using UnityEngine;

/// <summary>
/// Base class for agents in the simulation.
/// </summary>
/// <remarks>
/// This class defines the basic properties of agents in the simulation.
/// </remarks>
[RequireComponent(typeof(Rigidbody))]
public class BaseAgent : MonoBehaviour {
    [SerializeField] private BaseAgentData m_agentData;

    private Rigidbody m_rb;

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyePosition.position, eyeRadius);
    }

    /// <summary>
    /// Initializes the m_agent with provided data.
    /// </summary>
    /// <param name="t_baseAgent">The data to initialize the m_agent with.</param>
    public void init(BaseAgentData t_baseAgent) {
        gender = t_baseAgent.gender;
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
        angleChange = t_baseAgent.angleChange;
        circleDistance = t_baseAgent.circleDistance;
        circleRadius = t_baseAgent.circleRadius;
    }

    /// <summary>
    /// Retrieves the current velocity of the m_agent.
    /// </summary>
    /// <returns>The current velocity of the m_agent.</returns>
    public Vector3 getCurrentVelocity() {
        return m_rb.velocity;
    }

    /// <summary>
    /// Gets the mass of the m_agent.
    /// </summary>
    /// <returns>The mass of the m_agent.</returns>
    public float getMass() {
        return m_rb.mass;
    }

    /// <summary>
    /// Gets or sets the wander angle of the m_agent.
    /// </summary>
    /// <value>The wander angle of the m_agent.</value>
    public float wanderAngle {
        get { return m_agentData.wanderAngle; }
        set { m_agentData.wanderAngle = value; }
    }

    /// <summary>
    /// Gets or sets the gender of the m_agent.
    /// </summary>
    /// <value>The gender of the m_agent.</value>
    public Gender gender {
        get { return m_agentData.gender; }
        set { m_agentData.gender = value; }
    }

    /// <summary>
    /// Gets or sets the maximum speed of the m_agent.
    /// </summary>
    /// <value>The maximum speed of the m_agent.</value>
    public float maxSpeed {
        get { return m_agentData.maxSpeed; }
        set { m_agentData.maxSpeed = value; }
    }

    /// <summary>
    /// Gets or sets the maximum steering force of the m_agent.
    /// </summary>
    /// <value>The maximum steering force of the m_agent.</value>
    public float maxSteeringForce {
        get { return m_agentData.maxSteeringForce; }
        set { m_agentData.maxSteeringForce = value; }
    }

    /// <summary>
    /// Gets or sets the slowing radius of the m_agent.
    /// </summary>
    /// <value>The slowing radius of the m_agent.</value>
    public float slowingRadius {
        get { return m_agentData.slowingRadius; }
        set { m_agentData.slowingRadius = value; }
    }

    /// <summary>
    /// Gets or sets the target transform of the m_agent.
    /// </summary>
    /// <value>The target transform of the m_agent.</value>
    public Transform target {
        get { return m_agentData.target; }
        set { m_agentData.target = value; }
    }

    /// <summary>
    /// Gets or sets the target m_agent of the current m_agent.
    /// </summary>
    /// <value>The target m_agent of the current m_agent.</value>
    public BaseAgent targetAgent {
        get { return m_agentData.targetAgent; }
        set { m_agentData.targetAgent = value; }
    }

    /// <summary>
    /// Gets or sets the radius of the m_agent's "eye" for detecting obstacles or targets.
    /// </summary>
    /// <value>The radius of the m_agent's "eye".</value>
    public float eyeRadius {
        get { return m_agentData.eyeRadius; }
        set { m_agentData.eyeRadius = value; }
    }

    /// <summary>
    /// Gets or sets the position of the m_agent's "eye" for detecting obstacles or targets.
    /// </summary>
    /// <value>The position of the m_agent's "eye".</value>
    public Transform eyePosition {
        get { return m_agentData.eyePosition; }
        set { m_agentData.eyePosition = value; }
    }

    /// <summary>
    /// Gets or sets the angle change for steering behaviors.
    /// </summary>
    /// <value>The angle change for steering behaviors.</value>
    public float angleChange {
        get { return m_agentData.angleChange; }
        set { m_agentData.angleChange = value; }
    }

    /// <summary>
    /// Gets or sets the distance from the m_agent to maintain for circle-based behaviors.
    /// </summary>
    /// <value>The distance for circle-based behaviors.</value>
    public float circleDistance {
        get { return m_agentData.circleDistance; }
        set { m_agentData.circleDistance = value; }
    }

    /// <summary>
    /// Gets or sets the radius of the circle for circle-based behaviors.
    /// </summary>
    /// <value>The radius of the circle for circle-based behaviors.</value>
    public float circleRadius {
        get { return m_agentData.circleRadius; }
        set { m_agentData.circleRadius = value; }
    }

    /// <summary>
    /// Gets or sets the radius for obstacle avoidance behaviors.
    /// </summary>
    /// <value>The radius for obstacle avoidance behaviors.</value>
    public float collisionObstacleAvoidanceRadius {
        get { return m_agentData.collisionObstacleAvoidanceRadius; }
        set { m_agentData.collisionObstacleAvoidanceRadius = value; }
    }

    /// <summary>
    /// Gets or sets the force applied for collision avoidance behaviors.
    /// </summary>
    /// <value>The force applied for collision avoidance behaviors.</value>
    public float collisionAvoidanceForce {
        get { return m_agentData.collisionAvoidanceForce; }
        set { m_agentData.collisionAvoidanceForce = value; }
    }

    /// <summary>
    /// Gets or sets the maximum hunger level of the m_agent.
    /// </summary>
    /// <value>The maximum hunger level of the m_agent.</value>
    public float maxHunger {
        get { return m_agentData.maxHunger; }
        set { m_agentData.maxHunger = value; }
    }

    /// <summary>
    /// Gets or sets the hunger threshold for eating behaviors.
    /// </summary>
    /// <value>The hunger threshold for eating behaviors.</value>
    public float hungerTreshold {
        get { return m_agentData.hungerTreshold; }
        set { m_agentData.hungerTreshold = value; }
    }

    /// <summary>
    /// Gets or sets the rate of hunger increase per second.
    /// </summary>
    /// <value>The rate of hunger increase per second.</value>
    public float hungerRatePerSecond {
        get { return m_agentData.hungerRatePerSecond; }
        set { m_agentData.hungerRatePerSecond = value; }
    }

    /// <summary>
    /// Gets or sets the distance at which the m_agent can eat.
    /// </summary>
    /// <value>The distance at which the m_agent can eat.</value>
    public float eatDistance {
        get { return m_agentData.eatDistance; }
        set { m_agentData.eatDistance = value; }
    }

    /// <summary>
    /// Gets or sets the maximum thirst level of the m_agent.
    /// </summary>
    /// <value>The maximum thirst level of the m_agent.</value>
    public float maxThirst {
        get { return m_agentData.maxThirst; }
        set { m_agentData.maxThirst = value; }
    }

    /// <summary>
    /// Gets or sets the thirst threshold for drinking behaviors.
    /// </summary>
    /// <value>The thirst threshold for drinking behaviors.</value>
    public float thirstTreshold {
        get { return m_agentData.thirstTreshold; }
        set { m_agentData.thirstTreshold = value; }
    }

    /// <summary>
    /// Gets or sets the rate of thirst increase per second.
    /// </summary>
    /// <value>The rate of thirst increase per second.</value>
    public float thirstRatePerSecond {
        get { return m_agentData.thirstRatePerSecond; }
        set { m_agentData.thirstRatePerSecond = value; }
    }

    /// <summary>
    /// Gets or sets the distance at which the m_agent can drink.
    /// </summary>
    /// <value>The distance at which the m_agent can drink.</value>
    public float drinkingDistance {
        get { return m_agentData.drinkingDistance; }
        set { m_agentData.drinkingDistance = value; }
    }

    /// <summary>
    /// Gets or sets the gestation time in seconds for reproduction.
    /// </summary>
    /// <value>The gestation time in seconds for reproduction.</value>
    public float gestationTimeInSeconds {
        get { return m_agentData.gestationTimeInSeconds; }
        set { m_agentData.gestationTimeInSeconds = value; }
    }

    /// <summary>
    /// Gets or sets the distance for reproduction behaviors.
    /// </summary>
    /// <value>The distance for reproduction behaviors.</value>
    public float reproductionDistance {
        get { return m_agentData.reproductionDistance; }
        set { m_agentData.reproductionDistance = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the m_agent is pregnant.
    /// </summary>
    /// <value><c>true</c> if the m_agent is pregnant; otherwise, <c>false</c>.</value>
    public bool isPregnant {
        get { return m_agentData.isPregnant; }
        set {
            if (gender == Gender.Male) {
                return;
            }
            m_agentData.isPregnant = value;
        }
    }

    /// <summary>
    /// Gets or sets the attractiveness level of the m_agent.
    /// </summary>
    /// <value>The attractiveness level of the m_agent.</value>
    public float attractiveness {
        get { return m_agentData.attractiveness; }
        set { m_agentData.attractiveness = value; }
    }

    /// <summary>
    /// Gets or sets the reproduction threshold for reproduction behaviors.
    /// </summary>
    /// <value>The reproduction threshold for reproduction behaviors.</value>
    public float reproductionTreshold {
        get { return m_agentData.reproductionTreshold; }
        set { m_agentData.reproductionTreshold = value; }
    }

    /// <summary>
    /// Gets or sets the maximum number of babies the m_agent can have.
    /// </summary>
    /// <value>The maximum number of babies the m_agent can have.</value>
    public int maxBabies {
        get { return m_agentData.maxBabies; }
        set { m_agentData.maxBabies = value; }
    }

    /// <summary>
    /// Gets or sets the minimum number of babies the m_agent can have.
    /// </summary>
    /// <value>The minimum number of babies the m_agent can have.</value>
    public int minBabies {
        get { return m_agentData.minBabies; }
        set { m_agentData.minBabies = value; }
    }

    /// <summary>
    /// Gets or sets the average death age of the m_agent.
    /// </summary>
    /// <value>The average death age of the m_agent.</value>
    public float averageDeathAge {
        get { return m_agentData.averageDeathAge; }
        set { m_agentData.averageDeathAge = value; }
    }

    /// <summary>
    /// Gets or sets the rate of aging per second for the m_agent.
    /// </summary>
    /// <value>The rate of aging per second for the m_agent.</value>
    public float ageRatePerSecond {
        get { return m_agentData.ageRatePerSecond; }
        set { m_agentData.ageRatePerSecond = value; }
    }

    /// <summary>
    /// Gets or sets the age at which the m_agent becomes capable of reproduction.
    /// </summary>
    /// <value>The age at which the m_agent becomes capable of reproduction.</value>
    public float reproductionAge {
        get { return m_agentData.reproductionAge; }
        set { m_agentData.reproductionAge = value; }
    }

    /// <summary>
    /// Retrieves the data object containing information about the m_agent.
    /// </summary>
    /// <returns>The data object containing information about the m_agent.</returns>
    public BaseAgentData getBaseAgentData() {
        return m_agentData;
    }

    /// <summary>
    /// Gets or sets the current hunger level of the m_agent.
    /// </summary>
    /// <value>The current hunger level of the m_agent.</value>
    public float currentHunger {
        get { return m_agentData.currentHunger; }
        set { m_agentData.currentHunger = value; }
    }

    /// <summary>
    /// Gets or sets the current thirst level of the m_agent.
    /// </summary>
    /// <value>The current thirst level of the m_agent.</value>
    public float currentThirst {
        get { return m_agentData.currentThirst; }
        set { m_agentData.currentThirst = value; }
    }

    /// <summary>
    /// Gets or sets the current gestation time of the m_agent.
    /// </summary>
    /// <value>The current gestation time of the m_agent.</value>
    public float currentGestation {
        get { return m_agentData.currentGestation; }
        set { m_agentData.currentGestation = value; }
    }

    /// <summary>
    /// Gets or sets the current reproduction urge of the m_agent.
    /// </summary>
    /// <value>The current reproduction urge of the m_agent.</value>
    public float currentReproductionUrge {
        get { return m_agentData.currentReproductionUrge; }
        set { m_agentData.currentReproductionUrge = value; }
    }

    /// <summary>
    /// Gets or sets the current age of the m_agent.
    /// </summary>
    /// <value>The current age of the m_agent.</value>
    public float currentAge {
        get { return m_agentData.currentAge; }
        set { m_agentData.currentAge = value; }
    }
}

/// <summary>
/// Enumeration representing the gender of an m_agent.
/// </summary>
public enum Gender {
    Female,
    Male
}