using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a bear m_agent in the simulation, capable of performing various behaviors such as eating, drinking, reproducing, and dying.
/// </summary>
[RequireComponent(typeof(BaseAgent))]
[RequireComponent(typeof(MovementManager))]
public class BearAgent : MonoBehaviour, IEat, IReduceVitals, ILookForFood, ILookForWater, IDrink, ILookForMate, IDie {
    private BaseAgent m_agent;
    private MovementManager m_movementManager;
    private BearState m_bearState;
    private GameObject m_closestFood;
    private GameObject m_closestWater;
    private GameObject m_closestMate;
    private BaseAgentData m_fatherBaseAgentData;

    private void Start() {
        m_agent = GetComponent<BaseAgent>();
        m_movementManager = GetComponent<MovementManager>();
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
        Destroy(m_closestFood);
        m_closestFood = null;
        m_agent.target = null;
        m_movementManager.setMovementState(MovementState.None);
        m_bearState = BearState.None;
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
            m_movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        m_closestFood = tempClosestFood;
        m_agent.target = m_closestFood.transform;
        m_agent.targetAgent = m_closestFood.GetComponent<BaseAgent>();
    }

    /// <summary>
    /// Moves the m_agent towards the closest food.
    /// </summary>
    public void moveTowardFood() {
        if (m_closestFood == null) {
            lookForFood();
            return;
        }
        if (!m_closestFood.activeSelf) {
            m_closestFood = null;
            return;
        }
        if (Vector3.Distance(transform.position, m_closestFood.transform.position) <= m_agent.eatDistance) {
            m_bearState = BearState.Eating;
            return;
        }
        if (Vector3.Distance(transform.position, m_closestFood.transform.position) <= m_agent.eatDistance * 3) {
            m_movementManager.setMovementState(MovementState.Arriving);
            return;
        }
        m_movementManager.setMovementState(MovementState.Pursuing);
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
            m_bearState = BearState.Drinking;
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
        m_bearState = BearState.None;
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
            m_movementManager.setMovementState(MovementState.Wandering);
            return;
        }
        m_closestMate = tempClosestMate;
        m_agent.target = m_closestMate.transform;
        m_closestMate.GetComponent<BearAgent>().waitForMate();
        m_bearState = BearState.Reproduce;
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
            m_bearState = BearState.None;
            m_fatherBaseAgentData = m_closestMate.GetComponent<BaseAgent>().getBaseAgentData();
            m_closestMate.GetComponent<BearAgent>().resetState();
            return;
        }
        m_movementManager.setMovementState(MovementState.Arriving);
    }

    /// <summary>
    /// Signals the m_agent to wait for mating.
    /// </summary>
    public void waitForMate() {
        m_bearState = BearState.Reproduce;
        m_movementManager.setMovementState(MovementState.None);
    }

    /// <summary>
    /// Resets the state of the m_agent after mating.
    /// </summary>
    public void resetState() {
        m_bearState = BearState.None;
    }

    /// <summary>
    /// Manages the decision-making process of the m_agent, determining its current state based on its vital stats and needs.
    /// </summary>
    private void decisionManager() {
        if (isActing()) {
            return;
        }
        if (m_agent.currentHunger >= m_agent.hungerTreshold) {
            m_bearState = BearState.Hungry;
        } else if (m_agent.currentThirst >= m_agent.thirstTreshold) {
            m_bearState = BearState.Thirsty;
        } else if (m_agent.currentReproductionUrge >= m_agent.reproductionTreshold) {
            m_bearState = BearState.LookingForMate;
        } else {
            m_bearState = BearState.Wandering;
        }
    }

    /// <summary>
    /// Checks if the is currently engaged in any action that prevents it from making further decisions.
    /// </summary>
    /// <returns>True if the m_agent is currently eating, drinking, or reproducing; otherwise, false.</returns>
    private bool isActing() {
        return m_bearState == BearState.Eating ||
            m_bearState == BearState.Drinking ||
            m_bearState == BearState.Reproduce;
    }

    /// <summary>
    /// Executes the action corresponding to the m_agent's current state.
    /// </summary>
    private void act() {
        switch (m_bearState) {
            case BearState.None:
                break;
            case BearState.Wandering:
                m_movementManager.setMovementState(MovementState.Wandering);
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
    /// Enum defining the possible states of the bear m_agent.
    /// </summary>
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