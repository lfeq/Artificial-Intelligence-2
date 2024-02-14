using UnityEngine;

/// <summary>
/// Represents a node in a graph.
/// </summary>
public class Node : MonoBehaviour {
    [HideInInspector] public float shortestDistance;
    [HideInInspector] public Node previousNode;
    [HideInInspector] public float distanceToTarget;
    [HideInInspector] public float heuristic;

    [SerializeField] private GameObject[] neighbourNodes;
    [SerializeField] private Node northNode;
    [SerializeField] private Node eastNode;
    [SerializeField] private Node southNode;
    [SerializeField] private Node westNode;

    private float[] m_edgesWeights;

    private void Awake() {
        m_edgesWeights = new float[neighbourNodes.Length];
        for (int i = 0; i < neighbourNodes.Length; i++) {
            m_edgesWeights[i] = Vector3.Distance(transform.position, neighbourNodes[i].transform.position);
        }
        SetNeighbourNodesOrientation();
    }

    /// <summary>
    /// Gets the weights of the edges to the neighboring nodes.
    /// </summary>
    /// <returns>The weights of the edges to the neighboring nodes.</returns>
    public float[] GetEdgesWeights() {
        return m_edgesWeights;
    }

    /// <summary>
    /// Gets the neighboring nodes of this node.
    /// </summary>
    /// <returns>The neighboring nodes of this node.</returns>
    public Node[] GetNeighbourNodes() {
        Node[] nodes = new Node[neighbourNodes.Length];
        for (int i = 0; i < neighbourNodes.Length; i++) {
            nodes[i] = neighbourNodes[i].GetComponent<Node>();
        }
        return nodes;
    }

    /// <summary>
    /// Gets the node to the north of this node.
    /// </summary>
    /// <returns>The node to the north of this node.</returns>
    public Node GetNorthNode() {
        return northNode;
    }

    /// <summary>
    /// Gets the node to the east of this node.
    /// </summary>
    /// <returns>The node to the east of this node.</returns>
    public Node GetEastNode() {
        return eastNode;
    }

    /// <summary>
    /// Gets the node to the west of this node.
    /// </summary>
    /// <returns>The node to the west of this node.</returns>
    public Node GetWestNode() {
        return westNode;
    }

    /// <summary>
    /// Gets the node to the south of this node.
    /// </summary>
    /// <returns>The node to the south of this node.</returns>
    public Node GetSouthNode() {
        return southNode;
    }

    /// <summary>
    /// Sets the orientation of the neighboring nodes.
    /// </summary>
    private void SetNeighbourNodesOrientation() {
        for (int i = 0; i < neighbourNodes.Length; i++) {
            Vector3 distance = neighbourNodes[i].transform.position - transform.position;
            if (distance.x > 0) {
                eastNode = neighbourNodes[i].GetComponent<Node>();
            } else if (distance.x < 0) {
                westNode = neighbourNodes[i].GetComponent<Node>();
            } else if (distance.z > 0) {
                northNode = neighbourNodes[i].GetComponent<Node>();
            } else if (distance.z < 0) {
                southNode = neighbourNodes[i].GetComponent<Node>();
            }
        }
    }
}