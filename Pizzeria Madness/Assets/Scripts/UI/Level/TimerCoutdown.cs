using TMPro;
using UnityEngine;

public class TimerCoutdown : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text timerText;

    private LevelManager levelManager;
    private float timer;
    private float countdown;

    private void Start() {
        levelManager = LevelManager.instance;
        timer = levelManager.GetTimer();
        countdown = timer;
    }

    private void Update() {
        if (levelManager.GetLevelState() != LevelManager.LevelState.PlayerIsMoving) {
            countdown = timer;
            return;
        }
        if (countdown > 0) {
            countdown -= Time.deltaTime;
            var t0 = (int)countdown; // Total full seconds
            var m = t0 / 60; // Minutes
            var s = t0 - m * 60; // Remaining seconds
            var ms = (int)((countdown - t0) * 100); // Two most significant values of milliseconds
            timerText.text = $"{s:00}:{ms:00}";
        }
        if (countdown < 0) {
            levelManager.ChangeLevelState(LevelManager.LevelState.ShowingResults);
            return;
        } // Time is over
    }
}