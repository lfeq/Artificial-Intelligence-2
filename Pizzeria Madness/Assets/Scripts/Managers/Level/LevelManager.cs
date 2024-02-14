using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the level state and nodes.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LevelManager : MonoBehaviour {

    /// <summary>
    /// Singleton instance of LevelManager.
    /// </summary>
    public static LevelManager s_instance;

    [SerializeField] private Node[] nodes;
    [SerializeField] private GameObject targetIndicatorPrefab;
    [SerializeField] private StartLevelCountdown levelCountdown;
    [SerializeField] private float timer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Result result;
    [SerializeField] private float nextRoundTimeInSeconds = 8f;

    private Transform m_targetIndicator;
    private LevelState m_levelState;
    private Node m_targetNode;
    private Node m_startNode;
    private PlayerController m_playerController;
    private LineRenderer m_lineRenderer;

    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    private void Start() {
        m_targetIndicator = Instantiate(targetIndicatorPrefab).transform;
        m_playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        ChangeLevelState(LevelState.ShowingNewTarget);
    }

    /// <summary>
    /// Gets the node at the specified index.
    /// </summary>
    /// <param name="t_index">Index of the node.</param>
    /// <returns>The node at the specified index.</returns>
    public Node GetNodeAtIndex(int t_index) {
        return nodes[t_index];
    }

    /// <summary>
    /// Gets the current level state.
    /// </summary>
    /// <returns>The current level state.</returns>
    public LevelState GetLevelState() {
        return m_levelState;
    }

    /// <summary>
    /// Gets the target node.
    /// </summary>
    /// <returns>The target node.</returns>
    public Node GetTargetNode() {
        return m_targetNode;
    }

    /// <summary>
    /// Gets the m_timer for the level.
    /// </summary>
    /// <returns>The m_timer for the level.</returns>
    public float GetTimer() {
        return timer;
    }

    /// <summary>
    /// Gets the time for the next round.
    /// </summary>
    /// <returns>The time for the next round.</returns>
    public float GetRestartRoundTime() {
        return nextRoundTimeInSeconds;
    }

    /// <summary>
    /// Gets the start node.
    /// </summary>
    /// <returns>The start node.</returns>
    public Node GetStartNode() {
        return m_startNode;
    }

    /// <summary>
    /// Changes the level state.
    /// </summary>
    /// <param name="t_newState">The new level state.</param>
    public void ChangeLevelState(LevelState t_newState) {
        if (m_levelState == t_newState) {
            return;
        }
        m_levelState = t_newState;
        switch (m_levelState) {
            case LevelState.None:
                break;
            case LevelState.PlayerIsMoving:
                break;
            case LevelState.ShowingResults:
                ShowResults();
                break;
            case LevelState.ShowingNewTarget:
                m_lineRenderer.enabled = false;
                SetNewTarget();
                break;
            default:
                throw new UnityException("Invalid Level State");
        }
    }

    /// <summary>
    /// Sets a new target node.
    /// </summary>
    private void SetNewTarget() {
        bool isPickingNewTarget = true;
        while (isPickingNewTarget) {
            m_targetNode = nodes[Random.Range(0, nodes.Length)];
            if (m_targetNode != m_playerController.GetCurrentNode()) {
                isPickingNewTarget = false;
            }
        }
        m_targetIndicator.position = m_targetNode.transform.position + (Vector3.up * 0.2f);
        levelCountdown.StartCountDown();
        m_startNode = m_playerController.GetCurrentNode();
        if (m_startNode == null) {
            m_startNode = GetNodeAtIndex(0);
        }// First round
    }

    /// <summary>
    /// Shows the results of the level.
    /// </summary>
    private void ShowResults() {
        List<Node> path = new List<Node>();
        if (GameManager.s_instance.GetDifficulty() == Difficulty.Medium) {
            path = Pathfinding.Dijkstra(nodes, m_startNode, m_targetNode);
        } else if (GameManager.s_instance.GetDifficulty() == Difficulty.Hard) {
            path = Pathfinding.AStar(nodes, m_startNode, m_targetNode);
        } else {
            throw new System.Exception("Difficulty setting is broken");
        }
        m_lineRenderer.enabled = true;
        m_lineRenderer.positionCount = path.Count + 1;
        m_lineRenderer.SetPosition(0, m_startNode.transform.position + Vector3.up * 0.5f);
        for (int i = 0; i < path.Count; i++) {
            m_lineRenderer.SetPosition(i + 1, path[i].transform.position + Vector3.up * 0.5f);
        }
        result.ShowResult(m_lineRenderer.positionCount >= m_playerController.GetPathDistance(), m_playerController.GetPathDistance(), m_lineRenderer.positionCount);
    }

    /// <summary>
    /// Enum for level states
    /// </summary>
    public enum LevelState {
        None,
        PlayerIsMoving,
        ShowingResults,
        ShowingNewTarget
    }
}