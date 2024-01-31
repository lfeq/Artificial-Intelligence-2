using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour {
    [SerializeField] private Transform[] verticesPositions;
    [SerializeField] private float edgeRange = 10f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private AnimationCurve lineWidth;
    [SerializeField] private Gradient lineColor;

    private List<Node> m_nodes = new List<Node>();

    private void Start() {
        CreateNodes();
        CreateEdges();
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    for (int i = 0; i < verticesPositions.Length; i++) {
    //        Gizmos.DrawWireSphere(verticesPositions[i].position, edgeRange);
    //    }
    //}

    private void CreateNodes() {
        for (int i = 0; i < verticesPositions.Length; i++) {
            Transform currentVertice = verticesPositions[i];
            float distanceToLastNode = Vector3.Distance(currentVertice.position, verticesPositions[verticesPositions.Length - 1].position);
            currentVertice.gameObject.AddComponent<Node>();
            currentVertice.gameObject.GetComponent<Node>().SetHolistic((int)distanceToLastNode);
            TextMesh text = verticesPositions[i].GetComponentInChildren<TextMesh>();
            text.text = distanceToLastNode.ToString();
        }
    }

    private void CreateEdges() {
        for (int i = 0; i < verticesPositions.Length; i++) {
            Collider[] verticesConnected = Physics.OverlapSphere(verticesPositions[i].position, edgeRange);
            foreach (Collider col in verticesConnected) {
                if (col.transform == verticesPositions[i].transform) {
                    continue;
                }
                Edge newEdge = new Edge();
                Node tempNode = verticesPositions[i].GetComponent<Node>();
                float distanceToNode = Vector3.Distance(verticesPositions[i].position, col.transform.position);
                Vector3 nodeDistace = col.transform.position - verticesPositions[i].position;
                newEdge.setEdges(tempNode, col.GetComponent<Node>());
                newEdge.setCost((int)distanceToNode);
                tempNode.AddEdgeToEdgeList(newEdge);
                GameObject edgeRenderer = new GameObject("Edge from: " + verticesPositions[i].name + " to: " + col.transform.name);
                print("Edge from: " + verticesPositions[i].name + " to: " + col.transform.name + " distance: " + nodeDistace);
                edgeRenderer.transform.position = (verticesPositions[i].position + col.transform.position) / 2;
                LineRenderer edgeLineRenderer = edgeRenderer.AddComponent<LineRenderer>();
                edgeLineRenderer.material = lineMaterial;
                edgeLineRenderer.widthCurve = lineWidth;
                edgeLineRenderer.colorGradient = lineColor;
                edgeLineRenderer.SetPosition(0, col.transform.position);
                edgeLineRenderer.SetPosition(1, verticesPositions[i].position);
                TextMesh text = edgeRenderer.AddComponent<TextMesh>();
                text.characterSize = 0.32f;
                text.color = Color.black;
                text.text = distanceToNode.ToString();
            }
        }
    }
}