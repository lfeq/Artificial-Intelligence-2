using System;
using UnityEngine;

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

    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject deerPrefab;
    [SerializeField] private GameObject bearPrefab;

    private void Awake() {
        if (FindObjectOfType<AgentFactory>() != null &&
            FindObjectOfType<AgentFactory>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    public void spawnInitialAgents(int t_rabbitCount, int t_foxCount, int t_deerCount, int t_bearCount) {
        BaseAgent tempAgent;
        for (int i = 0; i < t_rabbitCount; i++) {
            tempAgent = Instantiate(rabbitPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onRabbitBorn?.Invoke();
        }
        for (int i = 0; i < t_foxCount; i++) {
            tempAgent = Instantiate(foxPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onFoxBorn?.Invoke();
        }
        for (int i = 0; i < t_deerCount; i++) {
            tempAgent = Instantiate(deerPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onDeerBorn?.Invoke();
        }
        for (int i = 0; i < t_bearCount; i++) {
            tempAgent = Instantiate(bearPrefab, CellularAutomata2D.s_instance.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
            onBearBorn?.Invoke();
        }
    }

    public void spawnAgent(string t_animalTag, BaseAgentData t_dadGenes, BaseAgentData t_momGenes, Vector3 t_spawnPosition) {
        switch (t_animalTag) {
            case "Rabbit":
                instantiateAgent(rabbitPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onRabbitBorn?.Invoke();
                break;
            case "Fox":
                instantiateAgent(foxPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onFoxBorn?.Invoke();
                break;
            case "Deer":
                instantiateAgent(deerPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onDeerBorn?.Invoke();
                break;
            case "Bear":
                instantiateAgent(bearPrefab, t_dadGenes, t_momGenes, t_spawnPosition);
                onBearBorn?.Invoke();
                break;
        }
    }

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

    private void instantiateAgent(GameObject t_agentPrefab, BaseAgentData t_dadGenes, BaseAgentData t_momGenes, Vector3 t_spawnPosition) {
        BaseAgent baby = Instantiate(t_agentPrefab, t_spawnPosition, Quaternion.identity).GetComponent<BaseAgent>();
        BaseAgentData genes = GeneticsManager.reproduce(t_dadGenes, t_momGenes);
        baby.init(genes);
    }
}