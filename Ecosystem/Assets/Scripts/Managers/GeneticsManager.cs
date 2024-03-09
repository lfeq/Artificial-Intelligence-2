using UnityEngine;

public class GeneticsManager {

    public static BaseAgentData reproduce(BaseAgent t_father, BaseAgent t_mother) {
        float mutationProbability = 0.05f;
        BaseAgentData sonAgent = new BaseAgentData();
        // 50/50 chance male or female
        sonAgent.genre = Random.value < 0.5f ? Genre.Male : Genre.Female; // 50/50 chance male or female
        // Set son maxSpeed
        sonAgent.maxSpeed = Random.value < 0.5f ? t_father.maxSpeed : t_mother.maxSpeed;
        sonAgent.maxSpeed = Random.value < mutationProbability ? t_father.maxSpeed * Random.Range(0.1f, 2f) : t_mother.maxSpeed * Random.Range(0.1f, 2f);
        // Set son maxSteeringForce
        sonAgent.maxSteeringForce = Random.value < 0.5f ? t_father.maxSteeringForce : t_mother.maxSteeringForce;
        sonAgent.maxSteeringForce = Random.value < mutationProbability ? t_father.maxSteeringForce * Random.Range(0.1f, 2f) : t_mother.maxSteeringForce * Random.Range(0.1f, 2f);
        // Set son maxSpeed
        sonAgent.slowingRadius = Random.value < 0.5f ? t_father.slowingRadius : t_mother.slowingRadius;
        sonAgent.slowingRadius = Random.value < mutationProbability ? t_father.slowingRadius * Random.Range(0.1f, 2f) : t_mother.slowingRadius * Random.Range(0.1f, 2f);
        // Set son eyeRadius
        sonAgent.eyeRadius = Random.value < 0.5f ? t_father.eyeRadius : t_mother.eyeRadius;
        sonAgent.eyeRadius = Random.value < mutationProbability ? t_father.eyeRadius * Random.Range(0.1f, 2f) : t_mother.eyeRadius * Random.Range(0.1f, 2f);
        // Set son collisionObstacleAvoidanceRadius
        sonAgent.collisionObstacleAvoidanceRadius = Random.value < 0.5f ? t_father.collisionObstacleAvoidanceRadius : t_mother.collisionObstacleAvoidanceRadius;
        sonAgent.collisionObstacleAvoidanceRadius = Random.value < mutationProbability ? t_father.collisionObstacleAvoidanceRadius * Random.Range(0.1f, 2f) : t_mother.collisionObstacleAvoidanceRadius * Random.Range(0.1f, 2f);
        // Set son collisionAvoidanceForce
        sonAgent.collisionAvoidanceForce = Random.value < 0.5f ? t_father.collisionAvoidanceForce : t_mother.collisionAvoidanceForce;
        sonAgent.collisionAvoidanceForce = Random.value < mutationProbability ? t_father.collisionAvoidanceForce * Random.Range(0.1f, 2f) : t_mother.collisionAvoidanceForce * Random.Range(0.1f, 2f);
        // Set son maxHunger
        sonAgent.maxHunger = Random.value < 0.5f ? t_father.maxHunger : t_mother.maxHunger;
        sonAgent.maxHunger = Random.value < mutationProbability ? t_father.maxHunger * Random.Range(0.1f, 2f) : t_mother.maxHunger * Random.Range(0.1f, 2f);
        // Set son hungerTreshold
        sonAgent.hungerTreshold = Random.value < 0.5f ? t_father.hungerTreshold : t_mother.hungerTreshold;
        sonAgent.hungerTreshold = Random.value < mutationProbability ? t_father.hungerTreshold * Random.Range(0.1f, 2f) : t_mother.hungerTreshold * Random.Range(0.1f, 2f);
        // Set son hungerRatePerSecond
        sonAgent.hungerRatePerSecond = Random.value < 0.5f ? t_father.hungerRatePerSecond : t_mother.hungerRatePerSecond;
        sonAgent.hungerRatePerSecond = Random.value < mutationProbability ? t_father.hungerRatePerSecond * Random.Range(0.1f, 2f) : t_mother.hungerRatePerSecond * Random.Range(0.1f, 2f);
        // Set son maxThirst
        sonAgent.maxThirst = Random.value < 0.5f ? t_father.maxThirst : t_mother.maxThirst;
        sonAgent.maxThirst = Random.value < mutationProbability ? t_father.maxThirst * Random.Range(0.1f, 2f) : t_mother.maxThirst * Random.Range(0.1f, 2f);
        // Set son thirstTreshold
        sonAgent.thirstTreshold = Random.value < 0.5f ? t_father.thirstTreshold : t_mother.thirstTreshold;
        sonAgent.thirstTreshold = Random.value < mutationProbability ? t_father.thirstTreshold * Random.Range(0.1f, 2f) : t_mother.thirstTreshold * Random.Range(0.1f, 2f);
        // Set son thirstRatePerSecond
        sonAgent.thirstRatePerSecond = Random.value < 0.5f ? t_father.thirstRatePerSecond : t_mother.thirstRatePerSecond;
        sonAgent.thirstRatePerSecond = Random.value < mutationProbability ? t_father.thirstRatePerSecond * Random.Range(0.1f, 2f) : t_mother.thirstRatePerSecond * Random.Range(0.1f, 2f);
        // Set son attractiveness
        sonAgent.attractiveness = Random.value < 0.5f ? t_father.attractiveness : t_mother.attractiveness;
        sonAgent.attractiveness = Random.value < mutationProbability ? t_father.attractiveness * Random.Range(0.1f, 2f) : t_mother.attractiveness * Random.Range(0.1f, 2f);
        return sonAgent;
    }
}