using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

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
        instance = this;
    }

    private void Start() {
        m_targetIndicator = Instantiate(targetIndicatorPrefab).transform;
        m_playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        ChangeLevelState(LevelState.ShowingNewTarget);
    }

    public Node GetNodeAtIndex(int t_index) {
        return nodes[t_index];
    }

    public LevelState GetLevelState() {
        return m_levelState;
    }

    public Node GetTargetNode() {
        return m_targetNode;
    }

    public float GetTimer() {
        return timer;
    }

    public float GetRestartRoundTime() {
        return nextRoundTimeInSeconds;
    }

    public Node GetStartNode() {
        return m_startNode;
    }

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

    public enum LevelState {
        None,
        PlayerIsMoving,
        ShowingResults,
        ShowingNewTarget
    }
}