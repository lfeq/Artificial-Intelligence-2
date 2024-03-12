using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject deerPrefab;
    [SerializeField] private GameObject bearPrefab;

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

    public GameObject getFoxPrefab() {
        return foxPrefab;
    }

    public GameObject getDeerPrefab() {
        return deerPrefab;
    }

    public GameObject getBearPrefab() {
        return bearPrefab;
    }
}