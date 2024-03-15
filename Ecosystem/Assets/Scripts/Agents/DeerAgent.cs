using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class DeerAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie, IHideFromHunter {
    private GameObject babyPrefab;
    public float currentHunger;
    public float currentThirst;
    public float currentGestation;
    public float currentReproductionUrge;
    public float currentAge;
    private BaseAgent agent;
    private MovementManager movementManager;
    [SerializeField] private DeerState deerState;
    private GameObject closestFood;
    private GameObject closestWater;
    private GameObject closestMate;
    private GameObject closestHunter;
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
        closestFood.SetActive(false);
        closestFood = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        deerState = DeerState.None;
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
        if (closestFood == null || agent.target == null) {
            lookForFood();
            return;
        }
        if (!closestFood.activeSelf) {
            closestFood = null;
            return;
        }
        if (Vector3.Distance(transform.position, closestFood.transform.position) <= agent.eatDistance) {
            deerState = DeerState.Eating;
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
            deerState = DeerState.Drinking;
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void drink() {
        currentThirst = 0;
        closestWater = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        deerState = DeerState.None;
    }

    public void lookForMate() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedPossibleMates = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag(gameObject.tag)) {
                if (col.GetComponent<BaseAgent>().genre == Genre.Male) {
                    percibedPossibleMates.Add(col.gameObject);
                } // Only add male rabbits
            }
        }
        float attractiviestMate = 0f;
        GameObject tempClosestMate = null;
        foreach (GameObject mate in percibedPossibleMates) {
            BaseAgent mateBaseAgent = mate.GetComponent<BaseAgent>();
            if (mate.GetComponent<DeerAgent>().currentAge <= mateBaseAgent.reproductionAge) {
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
        closestMate.GetComponent<DeerAgent>().waitForMate();
        deerState = DeerState.Reproduce;
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
            deerState = DeerState.None;
            fatherBaseAgentData = closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            closestMate.GetComponent<DeerAgent>().resetState();
            return;
        }
        movementManager.setMovementState(MovementState.Arriving);
    }

    public void waitForMate() {
        deerState = DeerState.Reproduce;
        movementManager.setMovementState(MovementState.None);
    }

    public void resetState() {
        deerState = DeerState.None;
    }

    public bool isCloseToHunter() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
        List<GameObject> percibedHunters = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag("Bear")) {
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
        }
        if (isCloseToHunter()) {
            deerState = DeerState.Scared;
        } else if (currentHunger >= agent.hungerTreshold) {
            deerState = DeerState.Hungry;
        } else if (currentThirst >= agent.thirstTreshold) {
            deerState = DeerState.Thirsty;
        } else if (currentReproductionUrge >= agent.reproductionTreshold) {
            deerState = DeerState.LookingForMate;
        } else {
            deerState = DeerState.Wandering;
        }
    }

    private bool isActing() {
        return deerState == DeerState.Eating ||
            deerState == DeerState.Drinking ||
            deerState == DeerState.Reproduce;
    }

    private void act() {
        switch (deerState) {
            case DeerState.None:
                break;
            case DeerState.Wandering:
                movementManager.setMovementState(MovementState.Wandering);
                break;
            case DeerState.Hungry:
                moveTowardFood();
                break;
            case DeerState.Eating:
                eat();
                break;
            case DeerState.Thirsty:
                moveTowardsWater();
                break;
            case DeerState.Drinking:
                drink();
                break;
            case DeerState.Reproduce:
                moveTorwardsMate();
                break;
            case DeerState.LookingForMate:
                lookForMate();
                break;
            case DeerState.Scared:
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
        currentGestation = 0f;
    }

    private enum DeerState {
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