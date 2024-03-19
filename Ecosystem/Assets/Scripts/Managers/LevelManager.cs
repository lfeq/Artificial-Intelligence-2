using UnityEngine;

/// <summary>
/// Manages the state and initialization of the game level.
/// </summary>
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

    /// <summary>
    /// Gets the current state of the level.
    /// </summary>
    /// <returns>The current state of the level.</returns>
    public LevelState getLevelState() {
        return m_levelState;
    }

    /// <summary>
    /// Sets the state of the level and performs necessary actions based on the state change.
    /// </summary>
    /// <param name="t_levelState">The new state of the level.</param>
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

    /// <summary>
    /// Loads the game level.
    /// </summary>
    private void loadLevel() {
        CellularAutomata2D.s_instance.generateAutomata();
    }

    /// <summary>
    /// Starts the simulation by spawning initial agents based on UI settings.
    /// </summary>
    private void startSimulation() {
        int rabbitCount = uiManager.getRabbitCount();
        int foxCount = uiManager.getFoxCount();
        int deerCount = uiManager.getDeerCount();
        int bearCount = uiManager.getBearCount();
        AgentFactory.s_instance.spawnInitialAgents(rabbitCount, foxCount, deerCount, bearCount);
    }
}

/// <summary>
/// Enumeration representing the possible states of the game level.
/// </summary>
public enum LevelState {
    None,
    LoadingLevel,
    Menu,
    Playing,
}