using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private BaseAgent currentBaseAgent;

    private void Update() {
        if (LevelManager.s_instance.getLevelState() != LevelState.Playing) {
            return;
        }
        selectAnimal();
        setStats();
    }

    private void selectAnimal() {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, animalLayerMask)) {
                GameObject selectedObject = hit.collider.gameObject;
                currentBaseAgent = selectedObject.GetComponent<BaseAgent>();
            }
        }
    }

    private void setStats() {
        if (currentBaseAgent == null) {
            return;
        }
        currentHungerIcon.fillAmount = currentBaseAgent.currentHunger / currentBaseAgent.maxHunger;
        currentThistIcon.fillAmount = currentBaseAgent.currentThirst / currentBaseAgent.maxThirst;
        currentReproductionUrgeIcon.fillAmount = currentBaseAgent.currentReproductionUrge / currentBaseAgent.reproductionTreshold;
        currentGenderText.text = $"Gender: {currentBaseAgent.gender.ToString()}";
        currentMaxSpeedText.text = $"Max Speed: {currentBaseAgent.maxSpeed}";
        currentEyeRadiusText.text = $"Eye Radius: {currentBaseAgent.eyeRadius}";
        currentAgeText.text = $"Age: {currentBaseAgent.currentAge}";
        currentAttractivnessText.text = $"Attractivness: {currentBaseAgent.attractiveness}";
    }
}