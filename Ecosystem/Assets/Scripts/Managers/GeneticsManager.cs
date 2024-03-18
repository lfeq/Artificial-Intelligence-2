using UnityEngine;

public class GeneticsManager {

    public static BaseAgentData reproduce(BaseAgentData t_father, BaseAgentData t_mother) {
        float mutationProbability = 0.1f;
        BaseAgentData sonAgent = new BaseAgentData();
        // 50/50 chance male or female
        sonAgent.gender = Random.value < 0.5f ? Gender.Male : Gender.Female; // 50/50 chance male or female
        // Set son maxSpeed
        sonAgent.maxSpeed = Random.value < 0.5f ? t_father.maxSpeed : t_mother.maxSpeed;
        if (Random.value < mutationProbability) {
            sonAgent.maxSpeed = Random.value < 0.5f ? t_father.maxSpeed * Random.Range(0.1f, 2f) : t_mother.maxSpeed * Random.Range(0.1f, 2f);
        }
        // Set son maxSteeringForce
        sonAgent.maxSteeringForce = Random.value < 0.5f ? t_father.maxSteeringForce : t_mother.maxSteeringForce;
        if (Random.value < mutationProbability) {
            sonAgent.maxSteeringForce = Random.value < 0.5f ? t_father.maxSteeringForce * Random.Range(0.1f, 2f) : t_mother.maxSteeringForce * Random.Range(0.1f, 2f);
        }
        // Set son maxSpeed
        sonAgent.slowingRadius = Random.value < 0.5f ? t_father.slowingRadius : t_mother.slowingRadius;
        if (Random.value < mutationProbability) {
            sonAgent.slowingRadius = Random.value < 0.5f ? t_father.slowingRadius * Random.Range(0.1f, 2f) : t_mother.slowingRadius * Random.Range(0.1f, 2f);
        }
        // Set son eyeRadius
        sonAgent.eyeRadius = Random.value < 0.5f ? t_father.eyeRadius : t_mother.eyeRadius;
        if (Random.value < mutationProbability) {
            sonAgent.eyeRadius = Random.value < 0.5f ? t_father.eyeRadius * Random.Range(0.1f, 2f) : t_mother.eyeRadius * Random.Range(0.1f, 2f);
        }
        // Set son collisionObstacleAvoidanceRadius
        sonAgent.collisionObstacleAvoidanceRadius = Random.value < 0.5f ? t_father.collisionObstacleAvoidanceRadius : t_mother.collisionObstacleAvoidanceRadius;
        if (Random.value < mutationProbability) {
            sonAgent.collisionObstacleAvoidanceRadius = Random.value < 0.5f ? t_father.collisionObstacleAvoidanceRadius * Random.Range(0.1f, 2f) : t_mother.collisionObstacleAvoidanceRadius * Random.Range(0.1f, 2f);
        }
        // Set son collisionAvoidanceForce
        sonAgent.collisionAvoidanceForce = Random.value < 0.5f ? t_father.collisionAvoidanceForce : t_mother.collisionAvoidanceForce;
        if (Random.value < mutationProbability) {
            sonAgent.collisionAvoidanceForce = Random.value < 0.5f ? t_father.collisionAvoidanceForce * Random.Range(0.1f, 2f) : t_mother.collisionAvoidanceForce * Random.Range(0.1f, 2f);
        }
        // Set son maxHunger
        sonAgent.maxHunger = Random.value < 0.5f ? t_father.maxHunger : t_mother.maxHunger;
        if (Random.value < mutationProbability) {
            sonAgent.maxHunger = Random.value < 0.5f ? t_father.maxHunger * Random.Range(0.1f, 2f) : t_mother.maxHunger * Random.Range(0.1f, 2f);
        }
        // Set son hungerTreshold
        sonAgent.hungerTreshold = Random.value < 0.5f ? t_father.hungerTreshold : t_mother.hungerTreshold;
        if (Random.value < mutationProbability) {
            sonAgent.hungerTreshold = Random.value < 0.5f ? t_father.hungerTreshold * Random.Range(0.1f, 2f) : t_mother.hungerTreshold * Random.Range(0.1f, 2f);
        }
        // Set son hungerRatePerSecond
        sonAgent.hungerRatePerSecond = Random.value < 0.5f ? t_father.hungerRatePerSecond : t_mother.hungerRatePerSecond;
        if (Random.value < mutationProbability) {
            sonAgent.hungerRatePerSecond = Random.value < 0.5f ? t_father.hungerRatePerSecond * Random.Range(0.1f, 2f) : t_mother.hungerRatePerSecond * Random.Range(0.1f, 2f);
        }
        // Set son maxThirst
        sonAgent.maxThirst = Random.value < 0.5f ? t_father.maxThirst : t_mother.maxThirst;
        if (Random.value < mutationProbability) {
            sonAgent.maxThirst = Random.value < 0.5f ? t_father.maxThirst * Random.Range(0.1f, 2f) : t_mother.maxThirst * Random.Range(0.1f, 2f);
        }
        // Set son thirstTreshold
        sonAgent.thirstTreshold = Random.value < 0.5f ? t_father.thirstTreshold : t_mother.thirstTreshold;
        if (Random.value < mutationProbability) {
            sonAgent.thirstTreshold = Random.value < 0.5f ? t_father.thirstTreshold * Random.Range(0.1f, 2f) : t_mother.thirstTreshold * Random.Range(0.1f, 2f);
        }
        // Set son thirstRatePerSecond
        sonAgent.thirstRatePerSecond = Random.value < 0.5f ? t_father.thirstRatePerSecond : t_mother.thirstRatePerSecond;
        if (Random.value < mutationProbability) {
            sonAgent.thirstRatePerSecond = Random.value < 0.5f ? t_father.thirstRatePerSecond * Random.Range(0.1f, 2f) : t_mother.thirstRatePerSecond * Random.Range(0.1f, 2f);
        }
        // Set son attractiveness
        sonAgent.attractiveness = Random.value < 0.5f ? t_father.attractiveness : t_mother.attractiveness;
        if (Random.value < mutationProbability) {
            sonAgent.attractiveness = Random.value < 0.5f ? t_father.attractiveness * Random.Range(0.1f, 2f) : t_mother.attractiveness * Random.Range(0.1f, 2f);
        }
        // Set son angle change
        sonAgent.angleChange = Random.value < 0.5f ? t_father.angleChange : t_mother.angleChange;
        if (Random.value < mutationProbability) {
            sonAgent.angleChange = Random.value < 0.5f ? t_father.angleChange * Random.Range(0.1f, 2f) : t_mother.angleChange * Random.Range(0.1f, 2f);
        }
        // Set son circleDistance
        sonAgent.circleDistance = Random.value < 0.5f ? t_father.circleDistance : t_mother.circleDistance;
        if (Random.value < mutationProbability) {
            sonAgent.circleDistance = Random.value < 0.5f ? t_father.circleDistance * Random.Range(0.1f, 2f) : t_mother.circleDistance * Random.Range(0.1f, 2f);
        }
        // Set son circleDistance
        sonAgent.circleRadius = Random.value < 0.5f ? t_father.circleRadius : t_mother.circleRadius;
        if (Random.value < mutationProbability) {
            sonAgent.circleRadius = Random.value < 0.5f ? t_father.circleRadius * Random.Range(0.1f, 2f) : t_mother.circleRadius * Random.Range(0.1f, 2f);
        }
        return sonAgent;
    }
}