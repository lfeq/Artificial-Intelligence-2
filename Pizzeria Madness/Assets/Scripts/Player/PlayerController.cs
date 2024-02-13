using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private AudioClip errorAudioClip;
    [SerializeField] private AudioClip successAudioClip;

    private LevelManager levelManager;
    private Node currentNode;
    private List<Node> pathNodes = new List<Node>();
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    private void Start() {
        levelManager = LevelManager.instance;
        currentNode = levelManager.GetNodeAtIndex(0);
        SetPositionToCurrentNode();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (levelManager.GetLevelState() == LevelManager.LevelState.ShowingNewTarget) {
            lineRenderer.enabled = false;
            pathNodes.Clear();
            return;
        }
        if (levelManager.GetLevelState() == LevelManager.LevelState.ShowingResults) {
            ShowResults();
            return;
        }
        Movement();
    }

    public List<Node> GetPathNodes() {
        return pathNodes;
    }

    public Node GetCurrentNode() {
        return currentNode;
    }

    public int GetPathDistance() {
        return pathNodes.Count + 1;
    }

    private void Movement() {
        Node newNode = null;
        bool keyPressed = false;
        if (Input.GetKeyUp(KeyCode.D)) {
            newNode = currentNode.GetEastNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.A)) {
            newNode = currentNode.GetWestNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.S)) {
            newNode = currentNode.GetSouthNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.W)) {
            newNode = currentNode.GetNorthNode();
            keyPressed = true;
        }
        if (!keyPressed) {
            return;
        }
        if (newNode == null) {
            PlayErrorSound();
            return;
        }
        currentNode = newNode;
        PlaySuccessSound();
        SetPositionToCurrentNode();
    }

    private void SetPositionToCurrentNode() {
        transform.position = currentNode.transform.position;
        pathNodes.Add(currentNode);
        if (currentNode == levelManager.GetTargetNode()) {
            levelManager.ChangeLevelState(LevelManager.LevelState.ShowingResults);
        }
    }

    private void ShowResults() {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = pathNodes.Count + 1;
        lineRenderer.SetPosition(0, levelManager.GetStartNode().transform.position + Vector3.up * 0.5f);
        for (int i = 0; i < pathNodes.Count; i++) {
            lineRenderer.SetPosition(i + 1, pathNodes[i].transform.position + Vector3.up * 0.5f);
        }
    }

    private void PlayErrorSound() {
        audioSource.clip = errorAudioClip;
        audioSource.pitch = Random.Range(1f, 3f);
        audioSource.Play();
    }

    private void PlaySuccessSound() {
        audioSource.clip = successAudioClip;
        audioSource.pitch = Random.Range(1f, 3f);
        audioSource.Play();
    }
}