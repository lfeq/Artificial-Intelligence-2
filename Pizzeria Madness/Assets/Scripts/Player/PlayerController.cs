using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement and interactions.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private AudioClip errorAudioClip;
    [SerializeField] private AudioClip successAudioClip;

    private LevelManager m_levelManager;
    private Node m_currentNode;
    private List<Node> m_pathNodes = new List<Node>();
    private LineRenderer m_lineRenderer;
    private AudioSource m_audioSource;

    private void Start() {
        m_levelManager = LevelManager.s_instance;
        m_currentNode = m_levelManager.GetNodeAtIndex(0);
        SetPositionToCurrentNode();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (m_levelManager.GetLevelState() == LevelManager.LevelState.ShowingNewTarget) {
            m_lineRenderer.enabled = false;
            m_pathNodes.Clear();
            return;
        }
        if (m_levelManager.GetLevelState() == LevelManager.LevelState.ShowingResults) {
            ShowResults();
            return;
        }
        Movement();
    }

    /// <summary>
    /// Gets the nodes in the path.
    /// </summary>
    /// <returns>The nodes in the path.</returns>
    public List<Node> GetPathNodes() {
        return m_pathNodes;
    }

    /// <summary>
    /// Gets the current node where the player is.
    /// </summary>
    /// <returns>The current node where the player is.</returns>
    public Node GetCurrentNode() {
        return m_currentNode;
    }

    /// <summary>
    /// Gets the distance of the path.
    /// </summary>
    /// <returns>The distance of the path.</returns>
    public int GetPathDistance() {
        return m_pathNodes.Count + 1;
    }

    /// <summary>
    /// Handles the player's movement.
    /// </summary>
    private void Movement() {
        Node newNode = null;
        bool keyPressed = false;
        if (Input.GetKeyUp(KeyCode.D)) {
            newNode = m_currentNode.GetEastNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.A)) {
            newNode = m_currentNode.GetWestNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.S)) {
            newNode = m_currentNode.GetSouthNode();
            keyPressed = true;
        } else if (Input.GetKeyUp(KeyCode.W)) {
            newNode = m_currentNode.GetNorthNode();
            keyPressed = true;
        }
        if (!keyPressed) {
            return;
        }
        if (newNode == null) {
            PlayErrorSound();
            return;
        }
        m_currentNode = newNode;
        PlaySuccessSound();
        SetPositionToCurrentNode();
    }

    /// <summary>
    /// Sets the player's position to the current node.
    /// </summary>
    private void SetPositionToCurrentNode() {
        transform.position = m_currentNode.transform.position;
        m_pathNodes.Add(m_currentNode);
        if (m_currentNode == m_levelManager.GetTargetNode()) {
            m_levelManager.ChangeLevelState(LevelManager.LevelState.ShowingResults);
        }
    }

    /// <summary>
    /// Shows the results of the level.
    /// </summary>
    private void ShowResults() {
        m_lineRenderer.enabled = true;
        m_lineRenderer.positionCount = m_pathNodes.Count + 1;
        m_lineRenderer.SetPosition(0, m_levelManager.GetStartNode().transform.position + Vector3.up * 0.5f);
        for (int i = 0; i < m_pathNodes.Count; i++) {
            m_lineRenderer.SetPosition(i + 1, m_pathNodes[i].transform.position + Vector3.up * 0.5f);
        }
    }

    /// <summary>
    /// Plays the error sound.
    /// </summary>
    private void PlayErrorSound() {
        m_audioSource.clip = errorAudioClip;
        m_audioSource.pitch = Random.Range(1f, 3f);
        m_audioSource.Play();
    }

    /// <summary>
    /// Plays the success sound.
    /// </summary>
    private void PlaySuccessSound() {
        m_audioSource.clip = successAudioClip;
        m_audioSource.pitch = Random.Range(1f, 3f);
        m_audioSource.Play();
    }
}