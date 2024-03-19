using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Observes the stats of the selected agent and updates the UI accordingly.
/// </summary>
public class AgentStatsObserver : MonoBehaviour {
    [SerializeField] private LayerMask animalLayerMask;
    [SerializeField] private Image currentHungerIcon;
    [SerializeField] private Image currentThistIcon;
    [SerializeField] private Image currentReproductionUrgeIcon;
    [SerializeField] private TMP_Text currentGenderText;
    [SerializeField] private TMP_Text currentMaxSpeedText;
    [SerializeField] private TMP_Text currentEyeRadiusText;
    [SerializeField] private TMP_Text currentAgeText;
    [SerializeField] private TMP_Text currentAttractivnessText;

    private BaseAgent m_currentBaseAgent;

    private void Update() {
        if (LevelManager.s_instance.getLevelState() != LevelState.Playing) {
            return;
        }
        selectAnimal();
        setStats();
    }

    /// <summary>
    /// Selects the agent clicked on by the player.
    /// </summary>
    private void selectAnimal() {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, animalLayerMask)) {
                GameObject selectedObject = hit.collider.gameObject;
                mcurrentBaseAgent = selectedObject.GetComponent<BaseAgent>();
            }
        }
    }

    /// <summary>
    /// Sets the UI elements to display the stats of the currently selected agent.
    /// </summary>
    private void setStats() {
        if (mcurrentBaseAgent == null) {
            return;
        }
        currentHungerIcon.fillAmount = mcurrentBaseAgent.currentHunger / mcurrentBaseAgent.maxHunger;
        currentThistIcon.fillAmount = mcurrentBaseAgent.currentThirst / mcurrentBaseAgent.maxThirst;
        currentReproductionUrgeIcon.fillAmount = mcurrentBaseAgent.currentReproductionUrge / mcurrentBaseAgent.reproductionTreshold;
        currentGenderText.text = $"Gender: {mcurrentBaseAgent.gender.ToString()}";
        currentMaxSpeedText.text = $"Max Speed: {mcurrentBaseAgent.maxSpeed}";
        currentEyeRadiusText.text = $"Eye Radius: {mcurrentBaseAgent.eyeRadius}";
        currentAgeText.text = $"Age: {mcurrentBaseAgent.currentAge}";
        currentAttractivnessText.text = $"Attractivness: {mcurrentBaseAgent.attractiveness}";
    }
}