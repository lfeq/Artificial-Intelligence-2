using UnityEngine;

/// <summary>
/// Manages the main menu audio and interactions.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MainMenuManager : MonoBehaviour {
    [SerializeField] private AudioClip hoverAudioClip, clickAudioClip;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    private void Start() {
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the hover audio.
    /// </summary>
    public void PlayHoverAudio() {
        m_audioSource.clip = hoverAudioClip;
        m_audioSource.Play();
    }

    /// <summary>
    /// Plays the click audio.
    /// </summary>
    public void PlayClickAudio() {
        m_audioSource.clip = clickAudioClip;
        m_audioSource.Play();
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitGame() {
        GameManager.s_instance.changeGameSate(GameState.QuitGame);
    }

    /// <summary>
    /// Sets the difficulty of the game.
    /// </summary>
    /// <param name="t_difficulty">The difficulty to set.</param>
    public void SetDifficulty(string t_difficulty) {
        GameManager.s_instance.changeDifficultyInEditor(t_difficulty);
    }
}