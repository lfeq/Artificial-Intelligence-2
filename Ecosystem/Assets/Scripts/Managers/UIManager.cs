using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField, Header("Sounds")] private AudioSource audioSource;
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField, Header("Inputs")] private TMP_InputField rabbitCount;
    [SerializeField] private TMP_InputField bearCount;
    [SerializeField] private TMP_InputField foxCount;
    [SerializeField] private TMP_InputField deerCount;

    public void playAudioClick() {
        audioSource.clip = clickAudioClip;
        audioSource.Play();
    }

    public void playAudioHover() {
        audioSource.clip = hoverAudioClip;
        audioSource.Play();
    }

    public void quitGame() {
        Application.Quit();
    }

    public void startSimulation() {
        LevelManager.s_instance.setLevelState(LevelState.Playing);
    }

    public int getRabbitCount() {
        return int.Parse(rabbitCount.text);
    }

    public int getBearCount() {
        return int.Parse(bearCount.text);
    }

    public int getFoxCount() {
        return int.Parse(foxCount.text);
    }

    public int getDeerCount() {
        return int.Parse(deerCount.text);
    }
}