using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    private Vector2 m_pos;
    private List<Edge> m_edges = new List<Edge>();
    private int m_holisitc;
    private Edge m_correctEdge;
    float m_caminoRecorrido;
    float m_cost;


    public Node() {
        m_caminoRecorrido = -1;
    }

    public Node(Vector2 t_pos, List<Edge> t_edges, int t_holistic) {
        m_pos = t_pos;
        m_edges = t_edges;
        m_holisitc = t_holistic;
        m_caminoRecorrido = -1;
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

    public float GetCost()
    {
        return m_cost;
    }

    public float GetCaminoRecorrido()
    {
        return m_caminoRecorrido;
    }

    public void SetCost(float t_cost)
    {
        m_cost = t_cost;
    }

    public void SetCaminoRecorrido(float t_caminoRecorrido)
    {
        m_caminoRecorrido = t_caminoRecorrido;
    }
}