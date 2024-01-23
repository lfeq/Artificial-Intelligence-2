using System;

[Serializable]
public class Edge {
    private Node m_from;
    private Node m_to;
    private int m_distance;
    private bool m_traversed;

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

    public void setEdges(Node t_from, Node t_to) {
        m_from = t_from;
        m_to = t_to;
    }

    public void setCost(int t_distance) {
        m_distance = t_distance;
    }

    public Node getNodeFrom() {
        return m_from;
    }

    public Node getNodeTo() {
        return m_to;
    }

    public int getDistance() {
        return m_distance;
    }
}