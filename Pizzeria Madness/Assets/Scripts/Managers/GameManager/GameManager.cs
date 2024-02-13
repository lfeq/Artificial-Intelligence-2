using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager s_instance;

    private GameState m_gameState;
    private Difficulty m_difficulty;
    private string m_newLevel;

    private void Awake() {
        if (s_instance != null && s_instance != this) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        s_instance = this;
        m_gameState = GameState.None;
        m_difficulty = Difficulty.None;
    }

    public void changeGameSate(GameState t_newState) {
        if (m_gameState == t_newState) {
            return;
        }
        m_gameState = t_newState;
        switch (m_gameState) {
            case GameState.None:
                break;
            case GameState.LoadMainMenu:
                loadMenu();
                break;
            case GameState.MainMenu:
                break;
            case GameState.LoadLevel:
                loadLevel();
                break;
            case GameState.Playing:
                break;
            case GameState.RestartLevel:
                restartLevel();
                break;
            case GameState.GameOver:
                break;
            case GameState.Credits:
                break;
            case GameState.QuitGame:
                quitGame();
                break;
            default:
                throw new UnityException("Invalid Game State");
        }
    }

    public void changeGameStateInEditor(string t_newState) {
        changeGameSate((GameState)System.Enum.Parse(typeof(GameState), t_newState));
    }

    public void changeDifficulty(Difficulty t_difficulty) {
        m_difficulty = t_difficulty;
        switch (m_difficulty) {
            case Difficulty.None:
                break;
            case Difficulty.Medium:
                changeGameSate(GameState.LoadLevel);
                break;
            case Difficulty.Hard:
                changeGameSate(GameState.LoadLevel);
                break;
            default:
                throw new UnityException("Invalid Difficulty");
        }
    }

    public void changeDifficultyInEditor(string t_newState) {
        changeDifficulty((Difficulty)System.Enum.Parse(typeof(Difficulty), t_newState));
    }

    public Difficulty GetDifficulty() {
        return m_difficulty;
    }

    public GameState getGameState() {
        return m_gameState;
    }

    public void setNewLevelName(string t_newLevel) {
        m_newLevel = t_newLevel;
    }

    public void loadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    private void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void loadLevel() {
        SceneManager.LoadScene("SampleScene");
    }

    private void quitGame() {
        Application.Quit();
    }
}

public enum GameState {
    None,
    LoadMainMenu,
    MainMenu,
    LoadLevel,
    Playing,
    RestartLevel,
    GameOver,
    Credits,
    QuitGame,
}

public enum Difficulty {
    None,
    Medium,
    Hard
}