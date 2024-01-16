using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphGenerator : MonoBehaviour {
    [SerializeField] private Transform[] verticesPositions;
    [SerializeField] private float edgeRange = 10f;

    private List<Node> m_nodes = new List<Node>();

    private void Start() {
        CreateNodes();
        CreateEdges();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        for (int i = 0; i < verticesPositions.Length; i++) {
            Gizmos.DrawWireSphere(verticesPositions[i].position, edgeRange);
        }
    }

    private void CreateNodes() {
        for (int i = 0; i < verticesPositions.Length; i++) {
            m_nodes.Add(verticesPositions[i].GetComponent<Node>());
        }
    }

    private void CreateEdges() {
        for (int i = 0; i < verticesPositions.Length; i++) {
            Collider[] verticesConnected = Physics.OverlapSphere(verticesPositions[i].position, edgeRange);
            foreach (Collider col in verticesConnected) {
                Edge newEdge = new Edge();
                newEdge.setEdges(verticesPositions[i].GetComponent<Node>(), col.GetComponent<Node>());
                verticesPositions[i].GetComponent<Node>().AddEdgeToEdgeList(newEdge);
            }
        }
    }
}