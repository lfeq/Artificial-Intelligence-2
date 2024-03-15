using UnityEngine;

//TODO: Agregar funcion para checar estado del levelmanager
public class LevelManager : MonoBehaviour {
    public static LevelManager s_instance;

    [SerializeField] private UIManager uiManager;

    private LevelState m_levelState;

    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    private void Start() {
        setLevelState(LevelState.LoadingLevel);
    }

    public LevelState getLevelState() {
        return m_levelState;
    }

    public void setLevelState(LevelState t_levelState) {
        if (m_levelState == t_levelState) {
            return;
        }
        m_levelState = t_levelState;
        switch (m_levelState) {
            case LevelState.None:
                break;
            case LevelState.LoadingLevel:
                loadLevel();
                break;
            case LevelState.Menu:
                break;
            case LevelState.Playing:
                startSimulation();
                break;
        }
    }

    private void loadLevel() {
        CellularAutomata2D.s_instance.generateAutomata();
    }

    private void startSimulation() {
        int rabbitCount = uiManager.getRabbitCount();
        int foxCount = uiManager.getFoxCount();
        int deerCount = uiManager.getDeerCount();
        int bearCount = uiManager.getBearCount();
        AgentFactory.s_instance.spawnInitialAgents(rabbitCount, foxCount, deerCount, bearCount);
    }
}

public enum LevelState {
    None,
    LoadingLevel,
    Menu,
    Playing,
}