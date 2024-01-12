using UnityEngine;

public class Edge {
    private Node m_from;
    private Node m_to;
    private int m_cost;

    public Edge() {
    }

    public Edge(ref Node t_from, ref Node t_to) {
        m_from = t_from;
        m_to = t_to;
    }

    public void setEdges(ref Node t_from, ref Node t_to) {
        m_from = t_from;
        m_to = t_to;
    }

    public void setCost(int t_cost) {
        m_cost = t_cost;
    }

    public Node getNodeFrom() {
        return m_from;
    }

    public Node getNodeTo() {
        return m_to;
    }
}