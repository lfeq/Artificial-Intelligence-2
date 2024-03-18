using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class RabbitAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie, IHideFromHunter {
    private BaseAgent agent;
    private MovementManager movementManager;
    [SerializeField] private RabbitState rabbitState;
    private GameObject closestFood;
    private GameObject closestWater;
    private GameObject closestMate;
    private GameObject closestHunter;
    private BaseAgentData fatherBaseAgentData;

    private void Start() {
        agent = GetComponent<BaseAgent>();
        movementManager = GetComponent<MovementManager>();
        agent.currentHunger = 0;
        agent.currentAge = 0;
    }

    private void Update() {
        reduceVitals();
        checkVitals();
        decisionManager();
        act();
    }

    public void eat() {
        agent.currentHunger = 0;
        closestFood.SetActive(false);
        closestFood = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        rabbitState = RabbitState.None;
    }

    public void reduceVitals() {
        agent.currentHunger += Time.deltaTime * agent.hungerRatePerSecond;
        agent.currentThirst += Time.deltaTime * agent.thirstRatePerSecond;
        agent.currentAge += Time.deltaTime * agent.ageRatePerSecond;
        if (agent.isPregnant) {
            agent.currentGestation += Time.deltaTime;
        }
        if (agent.gender == Gender.Female && !agent.isPregnant && agent.reproductionAge <= agent.currentAge) {
            agent.currentReproductionUrge += Time.deltaTime * agent.currentAge;
        }
    }

    public void checkVitals() {
        if (agent.currentGestation >= agent.gestationTimeInSeconds) {
            giveBirth();
        }
        if (agent.currentHunger >= agent.maxHunger ||
            agent.currentThirst >= agent.maxThirst ||
            agent.currentAge >= agent.averageDeathAge) {
            die();
        }
    }

    public void die() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        AgentFactory.s_instance.killAnimal(gameObject.tag);
    }

    public void lookForFood() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedFoods = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Bush") && col.gameObject.activeSelf) {
                percibedFoods.Add(col.gameObject);
            }
        }
        float closestFoodDistance = float.MaxValue;
        GameObject tempClosestFood = null;
        foreach (GameObject food in percibedFoods) {
            if (!food.activeSelf) {
                continue;
            }
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < closestFoodDistance) {
                tempClosestFood = food;
                closestFoodDistance = distance;
            }
        }
        if (tempClosestFood == null) {
            movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        closestFood = tempClosestFood;
        agent.target = closestFood.transform;
    }

    public void moveTowardFood() {
        if (closestFood == null) {
            lookForFood();
            return;
        }
        if (!closestFood.activeSelf) {
            closestFood = null;
            return;
        }
        if (Vector3.Distance(transform.position, closestFood.transform.position) <= agent.eatDistance) {
            rabbitState = RabbitState.Eating;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void lookForWater() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedWaterTiles = new List<GameObject>();
        TileController tileController;
        foreach (Collider col in percibed) {
            if (col.CompareTag("Obstacle")) {
                if (col.TryGetComponent<TileController>(out tileController) && tileController.getTileType() == TileType.Water) {
                    percibedWaterTiles.Add(col.gameObject);
                }
            }
        }
        float closestFoodDistance = float.MaxValue;
        GameObject tempClosestWater = null;
        foreach (GameObject water in percibedWaterTiles) {
            float distance = Vector3.Distance(transform.position, water.transform.position);
            if (distance < closestFoodDistance) {
                tempClosestWater = water;
                closestFoodDistance = distance;
            }
        }
        if (tempClosestWater == null) {
            movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        closestWater = tempClosestWater;
        agent.target = tempClosestWater.transform;
    }

    public void moveTowardsWater() {
        if (closestWater == null || agent.target == null) {
            lookForWater();
            return;
        }
        if (Vector3.Distance(transform.position, closestWater.transform.position) <= agent.drinkingDistance) {
            rabbitState = RabbitState.Drinking;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void drink() {
        agent.currentThirst = 0;
        closestWater = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        rabbitState = RabbitState.None;
    }

    public void lookForMate() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedPossibleMates = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Rabbit")) {
                if (col.GetComponent<BaseAgent>().gender == Gender.Male) {
                    percibedPossibleMates.Add(col.gameObject);
                } // Only add male rabbits
            }
        }
        float attractiviestMate = 0f;
        GameObject tempClosestMate = null;
        foreach (GameObject mate in percibedPossibleMates) {
            BaseAgent mateBaseAgent = mate.GetComponent<BaseAgent>();
            if (mateBaseAgent.currentAge <= mateBaseAgent.reproductionAge) {
                continue;
            }
            float tempAttractiveness = mate.GetComponent<BaseAgent>().attractiveness;
            if (tempAttractiveness > attractiviestMate) {
                tempClosestMate = mate;
                attractiviestMate = tempAttractiveness;
            }
        }
        if (tempClosestMate == null) {
            movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        closestMate = tempClosestMate;
        agent.target = closestMate.transform;
        closestMate.GetComponent<RabbitAgent>().waitForMate();
        rabbitState = RabbitState.Reproduce;
    }

    public void moveTorwardsMate() {
        if (agent.gender == Gender.Male) {
            movementManager.setMovementState(MovementState.None);
            return;
        }
        if (closestMate == null) {
            lookForMate();
            return;
        }
        if (Vector3.Distance(transform.position, closestMate.transform.position) <= agent.reproductionDistance) {
            agent.isPregnant = true;
            agent.currentReproductionUrge = 0;
            rabbitState = RabbitState.None;
            fatherBaseAgentData = closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            closestMate.GetComponent<RabbitAgent>().resetState();
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void waitForMate() {
        rabbitState = RabbitState.Reproduce;
        movementManager.setMovementState(MovementState.None);
    }

    public void resetState() {
        rabbitState = RabbitState.None;
    }

    public bool isCloseToHunter() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedHunters = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Fox")) {
                percibedHunters.Add(col.gameObject);
            }
        }
        GameObject tempClosestHunter = null;
        float tempClosestHunterDistance = float.MaxValue;
        foreach (GameObject hunter in percibedHunters) {
            BaseAgent mateBaseAgent = hunter.GetComponent<BaseAgent>();
            float distance = Vector3.Distance(transform.position, hunter.transform.position);
            if (tempClosestHunterDistance > distance) {
                tempClosestHunter = hunter;
                tempClosestHunterDistance = distance;
            }
        }

        if (tempClosestHunter == null) {
            return false;
        }
        closestHunter = tempClosestHunter;
        agent.target = tempClosestHunter.transform;
        agent.targetAgent = tempClosestHunter.GetComponent<BaseAgent>();
        return true;
    }

    public void hideFromHunter() {
        movementManager.setMovementState(MovementState.Evading);
    }

    private void decisionManager() {
        if (isActing()) {
            return;
        } // Rabbit is performing an action
        if (isCloseToHunter()) {
            rabbitState = RabbitState.Scared;
        } else if (agent.currentHunger >= agent.hungerTreshold) {
            rabbitState = RabbitState.Hungry;
        } else if (agent.currentThirst >= agent.thirstTreshold) {
            rabbitState = RabbitState.Thirsty;
        } else if (agent.currentReproductionUrge >= agent.reproductionTreshold) {
            rabbitState = RabbitState.LookingForMate;
        } else {
            rabbitState = RabbitState.Wandering;
        }
    }

    private bool isActing() {
        return rabbitState == RabbitState.Eating ||
            rabbitState == RabbitState.Drinking ||
            rabbitState == RabbitState.Reproduce;
    }

    private void act() {
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
            case RabbitState.Thirsty:
                moveTowardsWater();
                break;
            case RabbitState.Drinking:
                drink();
                break;
            case RabbitState.Reproduce:
                moveTorwardsMate();
                break;
            case RabbitState.LookingForMate:
                lookForMate();
                break;
            case RabbitState.Scared:
                hideFromHunter();
                break;
        }
    }

    private void giveBirth() {
        int numBabies = Random.Range(agent.minBabies, agent.maxBabies);
        for (int i = 0; i < numBabies; i++) {
            AgentFactory.s_instance.spawnAgent(gameObject.tag, fatherBaseAgentData, agent.getBaseAgentData(), transform.position);
        }
        closestMate = null;
        agent.isPregnant = false;
        agent.currentGestation = 0f;
    }

    private enum RabbitState {
        None,
        Wandering,
        Hungry,
        Eating,
        Thirsty,
        Drinking,
        Reproduce,
        LookingForMate,
        Scared
    }
}