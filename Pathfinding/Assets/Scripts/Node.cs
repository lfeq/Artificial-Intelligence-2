using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    private Vector2 m_pos;
    private List<Edge> m_edges = new List<Edge>();
    private int m_holisitc;
    private Edge m_correctEdge;

    public Node() {
    }

    public Node(Vector2 t_pos, List<Edge> t_edges, int t_holistic) {
        m_pos = t_pos;
        m_edges = t_edges;
        m_holisitc = t_holistic;
    }

    public void SetEdges(List<Edge> t_edges) {
        m_edges = t_edges;
    }

    public void SetHolistic(int t_holistic) {
        m_holisitc = t_holistic;
    }

    public void SetCorrectEdge(Edge t_edge) {
        m_correctEdge = t_edge;
    }

    public void AddEdgeToEdgeList(Edge t_edge) {
        m_edges.Add(t_edge);
    }

    public Edge GetCorrectEdge() {
        return m_correctEdge;
    }

    public List<Edge> GetEdges() {
        return m_edges;
    }

    public int GetHolisitc() {
        return m_holisitc;
    }
}