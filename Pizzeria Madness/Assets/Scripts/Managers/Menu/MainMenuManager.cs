using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainMenuManager : MonoBehaviour {
    [SerializeField] private AudioClip hoverAudioClip, clickAudioClip;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    private void Start() {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayHoverAudio() {
        m_audioSource.clip = hoverAudioClip;
        m_audioSource.Play();
    }

    public void PlayClickAudio() {
        m_audioSource.clip = clickAudioClip;
        m_audioSource.Play();
    }

    public void ExitGame() {
        GameManager.s_instance.changeGameSate(GameState.QuitGame);
    }

    public void SetDifficulty(string t_difficulty) {
        GameManager.s_instance.changeDifficultyInEditor(t_difficulty);
    }
}