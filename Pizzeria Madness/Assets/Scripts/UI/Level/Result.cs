using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display of results in the game.
/// </summary>
public class Result : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private string[] victoryTexts;
    [SerializeField] private string[] defeatTexts;

    private void Start() {
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// Shows the result of the game.
    /// </summary>
    /// <param name="t_didPlayerWin">Whether the player won.</param>
    /// <param name="t_playerPoints">The points of the player.</param>
    /// <param name="t_AIPoints">The points of the AI.</param>
    public void ShowResult(bool t_didPlayerWin, int t_playerPoints, int t_AIPoints) {
        canvasGroup.alpha = 1;
        string text = victoryTexts[Random.Range(0, victoryTexts.Length)];
        if (!t_didPlayerWin) {
            text = defeatTexts[Random.Range(0, defeatTexts.Length)];
        }
        dialogText.text = text;
        resultText.text = $"You: {t_playerPoints} \n AI: {t_AIPoints}";
        StartCoroutine(NextRound());
    }

    /// <summary>
    /// Starts the next round after a delay.
    /// </summary>
    private IEnumerator NextRound() {
        yield return new WaitForSeconds(LevelManager.s_instance.GetRestartRoundTime());
        LevelManager.s_instance.ChangeLevelState(LevelManager.LevelState.ShowingNewTarget);
        canvasGroup.alpha = 0;
    }
}