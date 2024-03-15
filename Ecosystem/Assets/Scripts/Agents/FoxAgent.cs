using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class FoxAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie {
    private GameObject babyPrefab;
    public float currentHunger;
    public float currentThirst;
    public float currentGestation;
    public float currentReproductionUrge;
    public float currentAge;
    private BaseAgent agent;
    private MovementManager movementManager;
    [SerializeField] private FoxState foxState;
    private GameObject closestFood;
    private GameObject closestWater;
    private GameObject closestMate;
    private BaseAgentData fatherBaseAgentData;

    private void Start() {
        agent = GetComponent<BaseAgent>();
        movementManager = GetComponent<MovementManager>();
        currentHunger = 0;
        currentAge = 0;
    }

    private void Update() {
        reduceVitals();
        checkVitals();
        decisionManager();
        act();
    }

    public void eat() {
        currentHunger = 0;
        Destroy(closestFood);
        closestFood = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        foxState = FoxState.None;
    }

    public void reduceVitals() {
        currentHunger += Time.deltaTime * agent.hungerRatePerSecond;
        currentThirst += Time.deltaTime * agent.thirstRatePerSecond;
        currentAge += Time.deltaTime * agent.ageRatePerSecond;
        if (agent.isPregnant) {
            currentGestation += Time.deltaTime;
        }
        if (agent.genre == Genre.Female && !agent.isPregnant && agent.reproductionAge <= currentAge) {
            currentReproductionUrge += Time.deltaTime * currentAge;
        }
    }

    public void checkVitals() {
        if (currentGestation >= agent.gestationTimeInSeconds) {
            giveBirth();
        }
        if (currentHunger >= agent.maxHunger ||
            currentThirst >= agent.maxThirst ||
            currentAge >= agent.averageDeathAge) {
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
            if (col.CompareTag("Rabbit") && col.gameObject.activeSelf) {
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
        agent.targetAgent = closestFood.GetComponent<BaseAgent>();
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
            foxState = FoxState.Eating;
            return;
        }
        if (Vector3.Distance(transform.position, closestFood.transform.position) <= agent.eatDistance * 3) {
            movementManager.setMovementState(MovementState.Arriving);
            return;
        }
        movementManager.setMovementState(MovementState.Pursuing);
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
            foxState = FoxState.Drinking;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void drink() {
        currentThirst = 0;
        closestWater = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        foxState = FoxState.None;
    }

    public void lookForMate() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedPossibleMates = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Fox")) {
                if (col.GetComponent<BaseAgent>().genre == Genre.Male) {
                    percibedPossibleMates.Add(col.gameObject);
                } // Only add male foxes
            }
        }
        float attractiviestMate = 0f;
        GameObject tempClosestMate = null;
        foreach (GameObject mate in percibedPossibleMates) {
            BaseAgent mateBaseAgent = mate.GetComponent<BaseAgent>();
            if (mate.GetComponent<FoxAgent>().currentAge <= mateBaseAgent.reproductionAge) {
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
        closestMate.GetComponent<FoxAgent>().waitForMate();
        foxState = FoxState.Reproduce;
    }

    public void moveTorwardsMate() {
        if (agent.genre == Genre.Male) {
            movementManager.setMovementState(MovementState.None);
            return;
        }
        if (closestMate == null) {
            lookForMate();
            return;
        }
        if (Vector3.Distance(transform.position, closestMate.transform.position) <= agent.reproductionDistance) {
            agent.isPregnant = true;
            currentReproductionUrge = 0;
            foxState = FoxState.None;
            fatherBaseAgentData = closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            closestMate.GetComponent<FoxAgent>().resetState();
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void waitForMate() {
        foxState = FoxState.Reproduce;
        movementManager.setMovementState(MovementState.None);
    }

    public void resetState() {
        foxState = FoxState.None;
    }

    private void decisionManager() {
        if (isActing()) {
            return;
        } // Rabbit is performing an action
        if (currentHunger >= agent.hungerTreshold) {
            foxState = FoxState.Hungry;
        } else if (currentThirst >= agent.thirstTreshold) {
            foxState = FoxState.Thirsty;
        } else if (currentReproductionUrge >= agent.reproductionTreshold) {
            foxState = FoxState.LookingForMate;
        } else {
            foxState = FoxState.Wandering;
        }
    }

    private bool isActing() {
        return foxState == FoxState.Eating ||
            foxState == FoxState.Drinking ||
            foxState == FoxState.Reproduce;
    }

    private void act() {
        switch (foxState) {
            case FoxState.None:
                break;
            case FoxState.Wandering:
                movementManager.setMovementState(MovementState.Wandering);
                break;
            case FoxState.Hungry:
                moveTowardFood();
                break;
            case FoxState.Eating:
                eat();
                break;
            case FoxState.Thirsty:
                moveTowardsWater();
                break;
            case FoxState.Drinking:
                drink();
                break;
            case FoxState.Reproduce:
                moveTorwardsMate();
                break;
            case FoxState.LookingForMate:
                lookForMate();
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
        currentGestation = 0f;
    }

    private enum FoxState {
        None,
        Wandering,
        Hungry,
        Eating,
        Thirsty,
        Drinking,
        Reproduce,
        LookingForMate,
    }
}