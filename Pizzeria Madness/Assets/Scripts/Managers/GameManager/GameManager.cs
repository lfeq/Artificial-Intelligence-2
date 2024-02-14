using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game state and difficulty.
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// Singleton instance of GameManager.
    /// </summary>
    public static GameManager s_instance;

    private GameState m_gameState;
    private Difficulty m_difficulty;

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

    /// <summary>
    /// Changes the game state.
    /// </summary>
    /// <param name="t_newState">New game state.
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

    /// <summary>
    /// Changes the game state in the editor.
    /// </summary>
    /// <param name="t_newState">New game state.
    public void changeGameStateInEditor(string t_newState) {
        changeGameSate((GameState)System.Enum.Parse(typeof(GameState), t_newState));
    }

    /// <summary>
    /// Changes the game difficulty.
    /// </summary>
    /// <param name="t_difficulty">New game difficulty.</param>
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

    /// <summary>
    /// Changes the game difficulty in the editor.
    /// </summary>
    /// <param name="t_newState">New game difficulty.</param>
    public void changeDifficultyInEditor(string t_newState) {
        changeDifficulty((Difficulty)System.Enum.Parse(typeof(Difficulty), t_newState));
    }

    /// <summary>
    /// Gets the current game difficulty.
    /// </summary>
    /// <returns>Current game difficulty.</returns>
    public Difficulty GetDifficulty() {
        return m_difficulty;
    }

    /// <summary>
    /// Gets the current game state.
    /// </summary>
    /// <returns>Current game state.</returns>
    public GameState getGameState() {
        return m_gameState;
    }

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void loadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    private void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the level.
    /// </summary>
    private void loadLevel() {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    private void quitGame() {
        Application.Quit();
    }
}

/// <summary>
/// Enum for game states.
/// </summary>
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

/// <summary>
/// Enum for game difficulties.
/// </summary>
public enum Difficulty {
    None,
    Medium,
    Hard
}