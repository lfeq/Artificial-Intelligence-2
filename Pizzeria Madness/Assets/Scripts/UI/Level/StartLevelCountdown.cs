using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the m_countdown at the start of the level.
/// </summary>
public class StartLevelCountdown : MonoBehaviour {
    [SerializeField] private TMP_Text startCountDownText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// Starts the m_countdown.
    /// </summary>
    public void StartCountDown() {
        StartCoroutine(StartCountdownCorutine());
    }

    /// <summary>
    /// Coroutine for the m_countdown.
    /// </summary>
    private IEnumerator StartCountdownCorutine() {
        canvasGroup.alpha = 1;
        int countdown = 3;
        audioSource.Play();
        while (countdown > 0) {
            startCountDownText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
            countdown--;
        }
        startCountDownText.text = "START";
        yield return new WaitForSeconds(1);
        canvasGroup.alpha = 0;
        LevelManager.s_instance.ChangeLevelState(LevelManager.LevelState.PlayerIsMoving);
    }
}