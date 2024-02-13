using System.Collections;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private string[] victoryTexts;
    [SerializeField] private string[] defeatTexts;

    private void Start() {
        canvasGroup.alpha = 0;
    }

    public void ShowResult(bool didPlayerWin, int playerPoints, int AIPoints) {
        canvasGroup.alpha = 1;
        string text = victoryTexts[Random.Range(0, victoryTexts.Length)];
        if (!didPlayerWin) {
            text = defeatTexts[Random.Range(0, defeatTexts.Length)];
        }
        dialogText.text = text;
        resultText.text = $"You: {playerPoints} \n AI: {AIPoints}";
        StartCoroutine(NextRound());
    }

    private IEnumerator NextRound() {
        yield return new WaitForSeconds(LevelManager.instance.GetRestartRoundTime());
        LevelManager.instance.ChangeLevelState(LevelManager.LevelState.ShowingNewTarget);
        canvasGroup.alpha = 0;
    }
}