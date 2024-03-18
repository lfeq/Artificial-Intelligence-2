using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class BearAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie {
    private BaseAgent agent;
    private MovementManager movementManager;
    [SerializeField] private BearState bearState;
    private GameObject closestFood;
    private GameObject closestWater;
    private GameObject closestMate;
    private BaseAgentData fatherBaseAgentData;

    private void Start() {
        agent = GetComponent<BaseAgent>();
        movementManager = GetComponent<MovementManager>();
    }

    private void Update() {
        reduceVitals();
        checkVitals();
        decisionManager();
        act();
    }

    public void eat() {
        agent.currentHunger = 0;
        Destroy(closestFood);
        closestFood = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        bearState = BearState.None;
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
            if ((col.CompareTag("Deer") || col.CompareTag("Rabbit")) && col.gameObject.activeSelf) {
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
            bearState = BearState.Eating;
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
            bearState = BearState.Drinking;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void drink() {
        agent.currentThirst = 0;
        closestWater = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        bearState = BearState.None;
    }

    public void lookForMate() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedPossibleMates = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag(gameObject.tag)) {
                if (col.GetComponent<BaseAgent>().gender == Gender.Male) {
                    percibedPossibleMates.Add(col.gameObject);
                } // Only add male foxes
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
        closestMate.GetComponent<BearAgent>().waitForMate();
        bearState = BearState.Reproduce;
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
            bearState = BearState.None;
            fatherBaseAgentData = closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            closestMate.GetComponent<BearAgent>().resetState();
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void waitForMate() {
        bearState = BearState.Reproduce;
        movementManager.setMovementState(MovementState.None);
    }

    public void resetState() {
        bearState = BearState.None;
    }

    private void decisionManager() {
        if (isActing()) {
            return;
        }
        if (agent.currentHunger >= agent.hungerTreshold) {
            bearState = BearState.Hungry;
        } else if (agent.currentThirst >= agent.thirstTreshold) {
            bearState = BearState.Thirsty;
        } else if (agent.currentReproductionUrge >= agent.reproductionTreshold) {
            bearState = BearState.LookingForMate;
        } else {
            bearState = BearState.Wandering;
        }
    }

    private bool isActing() {
        return bearState == BearState.Eating ||
            bearState == BearState.Drinking ||
            bearState == BearState.Reproduce;
    }

    private void act() {
        switch (bearState) {
            case BearState.None:
                break;
            case BearState.Wandering:
                movementManager.setMovementState(MovementState.Wandering);
                break;
            case BearState.Hungry:
                moveTowardFood();
                break;
            case BearState.Eating:
                eat();
                break;
            case BearState.Thirsty:
                moveTowardsWater();
                break;
            case BearState.Drinking:
                drink();
                break;
            case BearState.Reproduce:
                moveTorwardsMate();
                break;
            case BearState.LookingForMate:
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
        agent.currentGestation = 0f;
    }

    private enum BearState {
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