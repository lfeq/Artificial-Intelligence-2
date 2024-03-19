using TMPro;
using UnityEngine;

/// <summary>
/// Manages the UI interactions and provides methods to access input field values.
/// </summary>
public class UIManager : MonoBehaviour {
    [SerializeField, Header("Sounds")] private AudioSource audioSource;
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField, Header("Inputs")] private TMP_InputField rabbitCount;
    [SerializeField] private TMP_InputField bearCount;
    [SerializeField] private TMP_InputField foxCount;
    [SerializeField] private TMP_InputField deerCount;

    /// <summary>
    /// Plays the click sound effect.
    /// </summary>
    public void playAudioClick() {
        audioSource.clip = clickAudioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the hover sound effect.
    /// </summary>
    public void playAudioHover() {
        audioSource.clip = hoverAudioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void quitGame() {
        Application.Quit();
    }

    /// <summary>
    /// Starts the simulation by setting the level state to playing.
    /// </summary>
    public void startSimulation() {
        LevelManager.s_instance.setLevelState(LevelState.Playing);
    }

    /// <summary>
    /// Retrieves the count of rabbits entered in the input field.
    /// </summary>
    /// <returns>The count of rabbits.</returns>
    public int getRabbitCount() {
        return int.Parse(rabbitCount.text);
    }

    /// <summary>
    /// Retrieves the count of bears entered in the input field.
    /// </summary>
    /// <returns>The count of bears.</returns>
    public int getBearCount() {
        return int.Parse(bearCount.text);
    }

    /// <summary>
    /// Retrieves the count of foxes entered in the input field.
    /// </summary>
    /// <returns>The count of foxes.</returns>
    public int getFoxCount() {
        return int.Parse(foxCount.text);
    }

    /// <summary>
    /// Retrieves the count of deers entered in the input field.
    /// </summary>
    /// <returns>The count of deers.</returns>
    public int getDeerCount() {
        return int.Parse(deerCount.text);
    }
}