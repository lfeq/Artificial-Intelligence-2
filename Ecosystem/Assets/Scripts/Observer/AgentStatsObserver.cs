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
                m_currentBaseAgent = selectedObject.GetComponent<BaseAgent>();
            }
        }
    }

    /// <summary>
    /// Sets the UI elements to display the stats of the currently selected agent.
    /// </summary>
    private void setStats() {
        if (m_currentBaseAgent == null) {
            return;
        }
        currentHungerIcon.fillAmount = m_currentBaseAgent.currentHunger / m_currentBaseAgent.maxHunger;
        currentThistIcon.fillAmount = m_currentBaseAgent.currentThirst / m_currentBaseAgent.maxThirst;
        currentReproductionUrgeIcon.fillAmount = m_currentBaseAgent.currentReproductionUrge / m_currentBaseAgent.reproductionTreshold;
        currentGenderText.text = $"Gender: {m_currentBaseAgent.gender.ToString()}";
        currentMaxSpeedText.text = $"Max Speed: {m_currentBaseAgent.maxSpeed}";
        currentEyeRadiusText.text = $"Eye Radius: {m_currentBaseAgent.eyeRadius}";
        currentAgeText.text = $"Age: {m_currentBaseAgent.currentAge}";
        currentAttractivnessText.text = $"Attractivness: {m_currentBaseAgent.attractiveness}";
    }
}