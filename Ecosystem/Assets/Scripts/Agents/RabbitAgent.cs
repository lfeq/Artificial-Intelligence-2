using System.Collections.Generic;
using UnityEngine;

//TODO: Add Thirst and Reproduction need
[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class RabbitAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood {
    private float currentHunger;
    private BaseAgent agent;
    private MovementManager movementManager;
    private RabbitState rabbitState;
    private GameObject closestFood;

    private void Start() {
        agent = GetComponent<BaseAgent>();
        movementManager = GetComponent<MovementManager>();
        currentHunger = 0;
    }

    private void Update() {
        reduceVitals();
        decisionManager();
    }

    public void eat() {
        currentHunger = 0;
        closestFood = null;
        agent.setTargetTransform(null);
        movementManager.setMovementState(MovementState.None);
        rabbitState = RabbitState.None;
    }

    public void reduceVitals() {
        currentHunger += Time.deltaTime * agent.getHungerRatePerSecond();
    }

    public void lookForFood() {
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        List<GameObject> percibedFoods = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Bush")) {
                percibedFoods.Add(col.gameObject);
            }
        }
        float closestFoodDistance = float.MaxValue;
        GameObject tempClosestFood = null;
        foreach (GameObject food in percibedFoods) {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < closestFoodDistance) {
                tempClosestFood = food;
                closestFoodDistance = distance;
            }
        }
        if (tempClosestFood == null) {
            return;
        }
        closestFood = tempClosestFood;
        agent.setTargetTransform(closestFood.transform);
    }

    public void moveTowardFood() {
        if (closestFood == null) {
            lookForFood();
            return;
        }
        if (Vector3.Distance(transform.position, closestFood.transform.position) <= agent.getEatDistance()) {
            rabbitState = RabbitState.Eating;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    private void decisionManager() {
        if (currentHunger >= agent.getHungerTreshold() && rabbitState != RabbitState.Eating) {
            rabbitState = RabbitState.Hungry;
        } else if (rabbitState != RabbitState.Eating) {
            rabbitState = RabbitState.Wandering;
        }
        switch (rabbitState) {
            case RabbitState.None:
                break;
            case RabbitState.Wandering:
                movementManager.setMovementState(MovementState.Wandering);
                break;
            case RabbitState.Hungry:
                moveTowardFood();
                break;
            case RabbitState.Eating:
                eat();
                break;
        }
    }

    private enum RabbitState {
        None,
        Wandering,
        Hungry,
        Eating
    }
}