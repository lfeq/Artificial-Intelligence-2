using System;
using UnityEngine;

/// <summary>
/// Factory class responsible for spawning and managing agents in the simulation.
/// </summary>
public class AgentFactory : MonoBehaviour {
    public static AgentFactory s_instance;

    public event Action onRabbitBorn;
    public event Action onRabbitDead;
    public event Action onFoxBorn;
    public event Action onFoxDead;
    public event Action onDeerBorn;
    public event Action onDeerDead;
    public event Action onBearBorn;
    public event Action onBearDead;

    [SerializeField] private GameObject m_rabbitPrefab;
    [SerializeField] private GameObject m_foxPrefab;
    [SerializeField] private GameObject m_deerPrefab;
    [SerializeField] private GameObject m_bearPrefab;

    private void Awake() {
        if (FindObjectOfType<AgentFactory>() != null &&
            FindObjectOfType<AgentFactory>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    /// <summary>
    /// Spawns initial agents of various types.
    /// </summary>
    public void spawnInitialAgents(int t_rabbitCount, int t_foxCount, int t_deerCount, int t_bearCount) {
        BaseAgent tempAgent;
        for (int i = 0; i < t_rabbitCount; i++) {
            tempAgent = Instantiate(m_rabbitPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onRabbitBorn?.Invoke();
        }
        for (int i = 0; i < t_foxCount; i++) {
            tempAgent = Instantiate(m_foxPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onFoxBorn?.Invoke();
        }
        for (int i = 0; i < t_deerCount; i++) {
            tempAgent = Instantiate(m_deerPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onDeerBorn?.Invoke();
        }
        for (int i = 0; i < t_bearCount; i++) {
            tempAgent = Instantiate(m_bearPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onBearBorn?.Invoke();
        }
    }

    /// <summary>
    /// Spawns a new agent of the given animal type.
    /// </summary>
    public void spawnAgent(string t_animalTag, BaseAgentData t_dadGenes, BaseAgentData t_momGenes, Vector3 t_spawnPosition) {
        switch (t_animalTag) {
            case "Rabbit":
                instantiateAgent(m_rabbitPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onRabbitBorn?.Invoke();
                break;
            case "Fox":
                instantiateAgent(m_foxPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onFoxBorn?.Invoke();
                break;
            case "Deer":
                instantiateAgent(m_deerPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onDeerBorn?.Invoke();
                break;
            case "Bear":
                instantiateAgent(m_bearPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onBearBorn?.Invoke();
                break;
        }
    }

    /// <summary>
    /// Notifies the factory about the death of an animal.
    /// </summary>
    public void killAnimal(string t_animalTag) {
        switch (t_animalTag) {
            case "Rabbit":
                onRabbitDead?.Invoke();
                break;
            case "Fox":
                onFoxDead?.Invoke();
                break;
            case "Deer":
                onDeerDead?.Invoke();
                break;
            case "Bear":
                onBearDead?.Invoke();
                break;
        }
    }

    /// <summary>
    /// Instantiates an agent of the given prefab and initializes its genes.
    /// </summary>
    private void instantiateAgent(GameObject t_agentPrefab, BaseAgentData t_dadGenes, BaseAgentData t_momGenes, Vector3 t_spawnPosition) {
        BaseAgent baby = Instantiate(t_agentPrefab, t_spawnPosition, Quaternion.identity).GetComponent<BaseAgent>();
        BaseAgentData genes = GeneticsManager.reproduce(t_dadGenes, t_momGenes);
        baby.init(genes);
    }
}