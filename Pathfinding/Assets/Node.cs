using UnityEngine;
using System.Collections.Generic;

public class Node {
    private Vector2 m_pos;
    private List<Edge> m_edges;
    private int m_holisitc;

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
}