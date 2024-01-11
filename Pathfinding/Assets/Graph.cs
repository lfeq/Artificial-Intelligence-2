using UnityEngine;
using System.Collections.Generic;

public class Graph
{
    List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }

    public void Initialize()
    {
        Edge edge1 = new Edge();
        Edge edge2 = new Edge();
        List<Edge> node1EdgeList1 = new List<Edge>();
        node1EdgeList1.Add(edge1);
        node1EdgeList1.Add(edge2);
        Node node1 = new Node(new Vector2(2,3), node1EdgeList1, 10);
    }
}
