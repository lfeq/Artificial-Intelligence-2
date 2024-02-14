using TMPro;
using UnityEngine;

/// <summary>
/// Manages the countdown timer in the game.
/// </summary>
public class TimerCoutdown : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text timerText;

    private LevelManager m_levelManager;
    private float m_timer;
    private float m_countdown;

    private void Start() {
        m_levelManager = LevelManager.s_instance;
        m_timer = m_levelManager.GetTimer();
        m_countdown = m_timer;
    }

    private void Update() {
        if (m_levelManager.GetLevelState() != LevelManager.LevelState.PlayerIsMoving) {
            m_countdown = m_timer;
            return;
        }
        if (m_countdown > 0) {
            m_countdown -= Time.deltaTime;
            var t0 = (int)m_countdown; // Total full seconds
            var m = t0 / 60; // Minutes
            var s = t0 - m * 60; // Remaining seconds
            var ms = (int)((m_countdown - t0) * 100); // Two most significant values of milliseconds
            timerText.text = $"{s:00}:{ms:00}";
        }
        if (m_countdown < 0) {
            m_levelManager.ChangeLevelState(LevelManager.LevelState.ShowingResults);
            return;
        } // Time is over
    }
}