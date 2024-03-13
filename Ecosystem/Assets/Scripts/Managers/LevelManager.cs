using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager s_instance;

    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject deerPrefab;
    [SerializeField] private GameObject bearPrefab;
    [SerializeField] private CellularAutomata2D levelGenerator;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TMP_Text rabbitCountText;
    [SerializeField] private TMP_Text foxCountText;
    [SerializeField] private TMP_Text deerCountText;
    [SerializeField] private TMP_Text bearCountText;

    private LevelState m_levelState;
    private int rabbitCount;
    private int foxCount;
    private int deerCount;
    private int bearCount;

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

    public GameObject getRabbitPrefab() {
        return rabbitPrefab;
    }

    public GameObject getFoxPrefab() {
        return foxPrefab;
    }

    public GameObject getDeerPrefab() {
        return deerPrefab;
    }

    public GameObject getBearPrefab() {
        return bearPrefab;
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

    public void addAnimal(string t_animalTag) {
        switch (t_animalTag) {
            case "Rabbit":
                rabbitCount++;
                break;
            case "Fox":
                foxCount++;
                break;
            case "Deer":
                deerCount++;
                break;
            case "Bear":
                bearCount++;
                break;
        }
        setText();
    }

    public void deadAnimal(string t_animalTag) {
        switch (t_animalTag) {
            case "Rabbit":
                rabbitCount--;
                break;
            case "Fox":
                foxCount--;
                break;
            case "Deer":
                deerCount--;
                break;
            case "Bear":
                bearCount--;
                break;
        }
        setText();
    }

    private void loadLevel() {
        levelGenerator.generateAutomata();
    }

    private void startSimulation() {
        BaseAgent tempAgent;
        rabbitCount = uiManager.getRabbitCount();
        foxCount = uiManager.getFoxCount();
        deerCount = uiManager.getDeerCount();
        bearCount = uiManager.getBearCount();
        for (int i = 0; i <= rabbitCount; i++) {
            tempAgent = Instantiate(rabbitPrefab, levelGenerator.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.genre = (i % 2 == 0) ? Genre.Male : Genre.Female;
        }
        for (int i = 0; i <= foxCount; i++) {
            tempAgent = Instantiate(foxPrefab, levelGenerator.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.genre = (i % 2 == 0) ? Genre.Male : Genre.Female;
        }
        for (int i = 0; i <= deerCount; i++) {
            tempAgent = Instantiate(deerPrefab, levelGenerator.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.genre = (i % 2 == 0) ? Genre.Male : Genre.Female;
        }
        for (int i = 0; i <= bearCount; i++) {
            tempAgent = Instantiate(bearPrefab, levelGenerator.getRandomWalkableTileTransform().position, Quaternion.identity).GetComponent<BaseAgent>();
            tempAgent.genre = (i % 2 == 0) ? Genre.Male : Genre.Female;
        }
        setText();
    }

    private void setText() {
        rabbitCountText.text = $"Rabbits: {rabbitCount}";
        foxCountText.text = $"Foxes: {foxCount}";
        deerCountText.text = $"Deers: {deerCount}";
        bearCountText.text = $"Bears: {bearCount}";
    }
}

public enum LevelState {
    None,
    LoadingLevel,
    Menu,
    Playing,
}