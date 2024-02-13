using UnityEngine;

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

    private float[] edgesWeights;

    private void Awake() {
        edgesWeights = new float[neighbourNodes.Length];
        for (int i = 0; i < neighbourNodes.Length; i++) {
            edgesWeights[i] = Vector3.Distance(transform.position, neighbourNodes[i].transform.position);
        }
        SetNeighbourNodesOrientation();
    }

    public float[] GetEdgesWeights() {
        return edgesWeights;
    }

    public Node[] GetNeighbourNodes() {
        Node[] nodes = new Node[neighbourNodes.Length];
        for (int i = 0; i < neighbourNodes.Length; i++) {
            nodes[i] = neighbourNodes[i].GetComponent<Node>();
        }
        return nodes;
    }

    public Node GetNorthNode() {
        return northNode;
    }

    public Node GetEastNode() {
        return eastNode;
    }

    public Node GetWestNode() {
        return westNode;
    }

    public Node GetSouthNode() {
        return southNode;
    }

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