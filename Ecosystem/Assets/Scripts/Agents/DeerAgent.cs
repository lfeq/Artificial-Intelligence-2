using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a deer m_agent in the simulation, capable of performing various behaviors such as eating, drinking, reproducing, and dying.
/// </summary>
[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class DeerAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie, IHideFromHunter {
    private BaseAgent m_agent;
    private MovementManager m_movementManager;
    [SerializeField] private DeerState m_deerState;
    private GameObject m_closestFood;
    private GameObject m_closestWater;
    private GameObject m_closestMate;
    private GameObject m_closestHunter;
    private BaseAgentData m_fatherBaseAgentData;

    private void Start() {
        m_agent = GetComponent<BaseAgent>();
        m_movementManager = GetComponent<MovementManager>();
        m_agent.currentHunger = 0;
        m_agent.currentAge = 0;
    }

    private void Update() {
        reduceVitals();
        checkVitals();
        decisionManager();
        act();
    }

    /// <summary>
    /// Implements the eating behavior of the m_agent.
    /// </summary>
    public void eat() {
        m_agent.currentHunger = 0;
        m_closestFood.SetActive(false);
        m_closestFood = null;
        m_agent.target = null;
        m_movementManager.setMovementState(MovementState.None);
        m_deerState = DeerState.None;
    }

    /// <summary>
    /// Reduces the vital stats of the m_agent over time.
    /// </summary>
    public void reduceVitals() {
        m_agent.currentHunger += Time.deltaTime * m_agent.hungerRatePerSecond;
        m_agent.currentThirst += Time.deltaTime * m_agent.thirstRatePerSecond;
        m_agent.currentAge += Time.deltaTime * m_agent.ageRatePerSecond;
        if (m_agent.isPregnant) {
            m_agent.currentGestation += Time.deltaTime;
        }
        if (m_agent.gender == Gender.Female && !m_agent.isPregnant && m_agent.reproductionAge <= m_agent.currentAge) {
            m_agent.currentReproductionUrge += Time.deltaTime * m_agent.currentAge;
        }
    }

    /// <summary>
    /// Checks the vital stats of the m_agent and performs necessary actions.
    /// </summary>
    public void checkVitals() {
        if (m_agent.currentGestation >= m_agent.gestationTimeInSeconds) {
            giveBirth();
        }
        if (m_agent.currentHunger >= m_agent.maxHunger ||
            m_agent.currentThirst >= m_agent.maxThirst ||
            m_agent.currentAge >= m_agent.averageDeathAge) {
            die();
        }
    }

    /// <summary>
    /// Implements the death behavior of the m_agent.
    /// </summary>
    public void die() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        AgentFactory.s_instance.killAnimal(gameObject.tag);
    }

    /// <summary>
    /// Implements the food search behavior of the m_agent.
    /// </summary>
    public void lookForFood() {
        Collider[] percibed = Physics.OverlapSphere(m_agent.eyePosition.position, m_agent.eyeRadius);
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
            m_movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        m_closestFood = tempClosestFood;
        m_agent.target = m_closestFood.transform;
    }

    /// <summary>
    /// Moves the m_agent towards the closest food.
    /// </summary>
    public void moveTowardFood() {
        if (m_closestFood == null || m_agent.target == null) {
            lookForFood();
            return;
        }
        if (!m_closestFood.activeSelf) {
            m_closestFood = null;
            return;
        }
        if (Vector3.Distance(transform.position, m_closestFood.transform.position) <= m_agent.eatDistance) {
            m_deerState = DeerState.Eating;
            return;
        }
        m_movementManager.setMovementState(MovementState.Arriving);
    }

    /// <summary>
    /// Implements the water search behavior of the m_agent.
    /// </summary>
    public void lookForWater() {
        Collider[] percibed = Physics.OverlapSphere(m_agent.eyePosition.position, m_agent.eyeRadius);
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
            m_movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        m_closestWater = tempClosestWater;
        m_agent.target = tempClosestWater.transform;
    }

    /// <summary>
    /// Moves the m_agent towards the closest water source.
    /// </summary>
    public void moveTowardsWater() {
        if (m_closestWater == null || m_agent.target == null) {
            lookForWater();
            return;
        }
        if (Vector3.Distance(transform.position, m_closestWater.transform.position) <= m_agent.drinkingDistance) {
            m_deerState = DeerState.Drinking;
            return;
        }
        m_movementManager.setMovementState(MovementState.Arriving);
    }

    /// <summary>
    /// Implements the drinking behavior of the m_agent.
    /// </summary>
    public void drink() {
        m_agent.currentThirst = 0;
        m_closestWater = null;
        m_agent.target = null;
        m_movementManager.setMovementState(MovementState.None);
        m_deerState = DeerState.None;
    }

    /// <summary>
    /// Implements the mate search behavior of the m_agent.
    /// </summary>
    public void lookForMate() {
        Collider[] percibed = Physics.OverlapSphere(m_agent.eyePosition.position, m_agent.eyeRadius);
        List<GameObject> percibedPossibleMates = new List<GameObject>();
        foreach (Collider col in percibed) {
            if (col.CompareTag(gameObject.tag)) {
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
            m_movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        m_closestMate = tempClosestMate;
        m_agent.target = m_closestMate.transform;
        m_closestMate.GetComponent<DeerAgent>().waitForMate();
        m_deerState = DeerState.Reproduce;
    }

    /// <summary>
    /// Moves the m_agent towards the closest mate.
    /// </summary>
    public void moveTorwardsMate() {
        if (m_agent.gender == Gender.Male) {
            m_movementManager.setMovementState(MovementState.None);
            return;
        }
        if (m_closestMate == null) {
            lookForMate();
            return;
        }
        if (Vector3.Distance(transform.position, m_closestMate.transform.position) <= m_agent.reproductionDistance) {
            m_agent.isPregnant = true;
            m_agent.currentReproductionUrge = 0;
            m_deerState = DeerState.None;
            m_fatherBaseAgentData = m_closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            m_closestMate.GetComponent<DeerAgent>().resetState();
            return;
        }
        m_movementManager.setMovementState(MovementState.Arriving);
    }

    /// <summary>
    /// Signals the m_agent to wait for mating.
    /// </summary>
    public void waitForMate() {
        m_deerState = DeerState.Reproduce;
        m_movementManager.setMovementState(MovementState.None);
    }

    /// <summary>
    /// Resets the state of the m_agent after mating.
    /// </summary>
    public void resetState() {
        m_deerState = DeerState.None;
    }

    /// <summary>
    /// Checks if the m_agent is close to any hunter agents within its perception range.
    /// </summary>
    /// <returns>True if a hunter is detected within the agents's perception range; otherwise, false.</returns>
    public bool isCloseToHunter() {
        Collider[] percibed = Physics.OverlapSphere(m_agent.eyePosition.position, m_agent.eyeRadius);
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
        m_closestHunter = tempClosestHunter;
        m_agent.target = tempClosestHunter.transform;
        m_agent.targetAgent = tempClosestHunter.GetComponent<BaseAgent>();
        return true;
    }

    /// <summary>
    /// Initiates the action of hiding from the detected hunter by setting the movement state to evading.
    /// </summary>
    public void hideFromHunter() {
        m_movementManager.setMovementState(MovementState.Evading);
    }

    /// <summary>
    /// Manages the decision-making process of the m_agent, determining its current state based on its vital stats and needs.
    /// </summary>
    private void decisionManager() {
        if (isActing()) {
            return;
        }
        if (isCloseToHunter()) {
            m_deerState = DeerState.Scared;
        } else if (m_agent.currentHunger >= m_agent.hungerTreshold) {
            m_deerState = DeerState.Hungry;
        } else if (m_agent.currentThirst >= m_agent.thirstTreshold) {
            m_deerState = DeerState.Thirsty;
        } else if (m_agent.currentReproductionUrge >= m_agent.reproductionTreshold) {
            m_deerState = DeerState.LookingForMate;
        } else {
            m_deerState = DeerState.Wandering;
        }
    }

    /// <summary>
    /// Checks if the is currently engaged in any action that prevents it from making further decisions.
    /// </summary>
    /// <returns>True if the m_agent is currently eating, drinking, or reproducing; otherwise, false.</returns>
    private bool isActing() {
        return m_deerState == DeerState.Eating ||
            m_deerState == DeerState.Drinking ||
            m_deerState == DeerState.Reproduce;
    }

    /// <summary>
    /// Executes the action corresponding to the m_agent's current state.
    /// </summary>
    private void act() {
        switch (m_deerState) {
            case DeerState.None:
                break;
            case DeerState.Wandering:
                m_movementManager.setMovementState(MovementState.Wandering);
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

    /// <summary>
    /// Gives birth to a random number of baby agents with inherited traits from both parents.
    /// </summary>
    private void giveBirth() {
        int numBabies = Random.Range(m_agent.minBabies, m_agent.maxBabies);
        for (int i = 0; i < numBabies; i++) {
            AgentFactory.s_instance.spawnAgent(gameObject.tag, m_fatherBaseAgentData, m_agent.getBaseAgentData(), transform.position);
        }
        m_closestMate = null;
        m_agent.isPregnant = false;
        m_agent.currentGestation = 0f;
    }

    /// <summary>
    /// Enum defining the possible states of the deer m_agent.
    /// </summary>
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