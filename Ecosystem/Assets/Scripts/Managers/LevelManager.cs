using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    [SerializeField] private GameObject rabbitPrefab;

    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject getRabbitPrefab() {
        return rabbitPrefab;
    }
}