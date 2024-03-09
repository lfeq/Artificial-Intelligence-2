using System.Collections.Generic;
using UnityEngine;

//TODO: Add Reproduction
[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class RabbitAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate {
    public GameObject babyPrefab;

    public float currentHunger;
    public float currentThirst;
    public float currentGestation;
    public float currentReproductionUrge;
    private BaseAgent agent;
    private MovementManager movementManager;
    private RabbitState rabbitState;
    private GameObject closestFood;
    private GameObject closestWater;
    private GameObject closestMate;

    private void Start() {
        agent = GetComponent<BaseAgent>();
        movementManager = GetComponent<MovementManager>();
        currentHunger = 0;
        babyPrefab = LevelManager.instance.getRabbitPrefab();
    }

    private void Update() {
        reduceVitals();
        decisionManager();
        act();
    }

    public void eat() {
        currentHunger = 0;
        closestFood = null;
        agent.target = null;
        movementManager.setMovementState(MovementState.None);
        rabbitState = RabbitState.None;
    }

    public void reduceVitals() {
        currentHunger += Time.deltaTime * agent.hungerRatePerSecond;
        currentThirst += Time.deltaTime * agent.thirstRatePerSecond;
        if (agent.isPregnant) {
            currentGestation += Time.deltaTime;
            if (currentGestation >= agent.gestationTimeInSeconds) {
                giveBirth();
            }
        }
        if (agent.genre == Genre.Female && !agent.isPregnant) {
            currentReproductionUrge += Time.deltaTime; // TODO: Hacer variable con la edad
        }
    }

    public void lookForFood() {
        Collider[] percibed = Physics.OverlapSphere(agent.eyePosition.position, agent.eyeRadius);
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
        agent.target = closestFood.transform;
    }

    public void moveTowardFood() {
        if (closestFood == null) {
            lookForFood();
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
            return;
        }
        closestWater = tempClosestWater;
        agent.target = closestWater.transform;
    }

    public void moveTowardsWater() {
        if (closestWater == null) {
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
        currentThirst = 0;
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
                if (col.GetComponent<BaseAgent>().genre == Genre.Male) {
                    percibedPossibleMates.Add(col.gameObject);
                } // Only add male rabbits
            }
        }
        float attractiviestMate = 0f;
        GameObject tempClosestMate = null;
        foreach (GameObject mate in percibedPossibleMates) {
            float tempAttractiveness = mate.GetComponent<BaseAgent>().attractiveness;
            if (tempAttractiveness > attractiviestMate) {
                tempClosestMate = mate;
                attractiviestMate = tempAttractiveness;
            }
        }
        if (tempClosestMate == null) {
            return;
        }
        closestMate = tempClosestMate;
        agent.target = closestMate.transform;
        closestMate.GetComponent<RabbitAgent>().waitForMate();
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
            rabbitState = RabbitState.None;
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

    private void decisionManager() {
        if (isActing()) {
            return;
        } // Rabbit is performing an action
        if (currentReproductionUrge >= agent.reproductionTreshold) {
            rabbitState = RabbitState.Reproduce;
        } else if (currentHunger >= agent.hungerTreshold) {
            rabbitState = RabbitState.Hungry;
        } else if (currentThirst >= agent.thirstTreshold) {
            rabbitState = RabbitState.Thirsty;
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
        }
    }

    private void giveBirth() {
        int numBabies = Random.Range(agent.minBabies, agent.maxBabies);
        //BaseAgent father = closestMate.GetComponent<BaseAgent>();
        //for (int i = 0; i < numBabies; i++) {
        //    BaseAgent baby = Instantiate(babyPrefab, transform.position, Quaternion.identity).GetComponent<BaseAgent>();
        //    //BaseAgentData genes = GeneticsManager.reproduce(father, agent);
        //    BaseAgentData genes = GeneticsManager.reproduce(agent, agent);
        //    baby.init(genes);
        //}
        BaseAgent baby = Instantiate(babyPrefab, transform.position, Quaternion.identity).GetComponent<BaseAgent>();
        //BaseAgentData genes = GeneticsManager.reproduce(father, agent);
        BaseAgentData genes = GeneticsManager.reproduce(agent, agent);
        baby.init(genes);
        closestMate = null;
        agent.isPregnant = false;
        currentGestation = 0f;
    }

    private enum RabbitState {
        None,
        Wandering,
        Hungry,
        Eating,
        Thirsty,
        Drinking,
        Reproduce,
    }
}