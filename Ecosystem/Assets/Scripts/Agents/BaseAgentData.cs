using System;
using UnityEngine;

/// <summary>
/// Serializable data class containing information about a base m_agent.
/// </summary>
[Serializable]
public class BaseAgentData {
    [HideInInspector] public float wanderAngle = 0.5f;
    [Header("Genre")] public Gender gender;
    [Header("Movement")] public float maxSpeed = 5;
    public float maxSteeringForce = 5;
    public float slowingRadius = 5;
    [Header("Target")] public Transform target;
    public BaseAgent targetAgent;
    [Header("Sight")] public float eyeRadius = 3;
    public Transform eyePosition;
    [Header("Wander")] public float angleChange = 5;
    public float circleDistance = 5, circleRadius = 1;
    [Header("Collision Avoidance")] public float collisionObstacleAvoidanceRadius = 5;
    public float collisionAvoidanceForce = 5;
    [Header("Hunger")] public float maxHunger = 100;
    public float hungerTreshold = 50; //Treshold when m_agent starts to starve
    public float hungerRatePerSecond = 0.3f;
    public float eatDistance = 2f;
    public float currentHunger;
    [Header("Thirst")] public float maxThirst = 100;
    public float thirstTreshold = 50;
    public float thirstRatePerSecond = 0.3f;
    public float drinkingDistance = 2f;
    public float currentThirst;
    [Header("Reproduction")] public float gestationTimeInSeconds = 1000;
    public float reproductionDistance = 2;
    public bool isPregnant = false;
    public float attractiveness = 50f;
    public float reproductionTreshold = 50;
    public int maxBabies = 5;
    public int minBabies = 1;
    public float currentGestation;
    public float currentReproductionUrge;
    [Header("Age")] public float averageDeathAge = 7f;
    public float ageRatePerSecond = 1f;
    public float reproductionAge = 5f;
    public float currentAge;
}