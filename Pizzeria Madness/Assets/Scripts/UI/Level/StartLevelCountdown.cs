using System.Collections;
using TMPro;
using UnityEngine;

public class StartLevelCountdown : MonoBehaviour {
    [SerializeField] private TMP_Text startCountDownText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioSource audioSource;

    public void StartCountDown() {
        StartCoroutine(StartCountdownCorutine());
    }

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
        LevelManager.instance.ChangeLevelState(LevelManager.LevelState.PlayerIsMoving);
    }
}