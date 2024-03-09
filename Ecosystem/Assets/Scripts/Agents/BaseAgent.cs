using Unity.VisualScripting;
using UnityEngine;

//TODO: Add Reproduction
[RequireComponent(typeof(Rigidbody))]
public class BaseAgent : MonoBehaviour {

    #region Public variables

    [HideInInspector] public float wanderAngle = 0.5f;
    [Header("Genre")] public Genre genre;

    #endregion Public variables

    #region Serializable variables

    [Header("Movement")] public float maxSpeed = 5;
    public float maxSteeringForce = 5;
    public float slowingRadius = 5;
    public Transform target;
    [Header("Sight")] public float eyeRadius = 3;
    public Transform eyePosition;
    [Header("Wander")] public float angleChange = 5;
    public float circleDistance = 5, circleRadius = 1;
    [Header("Collision Avoidance")] public float collisionObstacleAvoidanceRadius = 5;
    public float collisionAvoidanceForce = 5;
    [Header("Hunger")] public float maxHunger = 100;
    public float hungerTreshold = 50; //Treshold when agent starts to starve
    public float hungerRatePerSecond = 0.3f;
    public float eatDistance = 2f;
    [Header("Thirst")] public float maxThirst = 100;
    public float thirstTreshold = 50;
    public float thirstRatePerSecond = 0.3f;
    public float drinkingDistance = 2f;
    [Header("Reproduction")] public float gestationTimeInSeconds = 1000;
    public float reproductionDistance = 2;
    public bool isPregnant = false;
    public float attractiveness = 50f;
    public float reproductionTreshold = 50;
    public int maxBabies = 5;
    public int minBabies = 1;

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

    public void init(BaseAgent t_baseAgent) {
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
}

public enum Genre {
    Female,
    Male
}