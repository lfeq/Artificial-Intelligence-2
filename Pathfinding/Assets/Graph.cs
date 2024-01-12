using UnityEngine;
using System.Collections.Generic;

public class Graph {
    private List<Node> nodes;

    public Graph() {
        nodes = new List<Node>();
    }

    public void Initialize() {
        // Create edges.
        Edge edge1 = new Edge();
        Edge edge2 = new Edge();
        Edge edge3 = new Edge();
        Edge edge4 = new Edge();
        Edge edge5 = new Edge();
        Edge edge6 = new Edge();
        Edge edge7 = new Edge();
        Edge edge8 = new Edge();
        Edge edge9 = new Edge();
        Edge edge10 = new Edge();
        Edge edge11 = new Edge();
        // Create vertices.
        Node node1 = new Node();
        Node node2 = new Node();
        Node node3 = new Node();
        Node node4 = new Node();
        Node node5 = new Node();
        Node node6 = new Node();
        Node node7 = new Node();
        Node node8 = new Node();
        Node node9 = new Node();
        // Set edges direction, start node and end node..
        edge1.setEdges(ref node1, ref node2);
        edge2.setEdges(ref node1, ref node3);
        edge3.setEdges(ref node3, ref node2);
        edge4.setEdges(ref node3, ref node4);
        edge5.setEdges(ref node4, ref node6);
        edge6.setEdges(ref node4, ref node5);
        edge7.setEdges(ref node5, ref node8);
        edge8.setEdges(ref node6, ref node2);
        edge9.setEdges(ref node2, ref node7);
        edge10.setEdges(ref node7, ref node9);
        edge11.setEdges(ref node8, ref node9);
        // Set edges cost
        edge1.setCost(3);
        edge2.setCost(1);
        edge3.setCost(5);
        edge4.setCost(1);
        edge5.setCost(1);
        edge6.setCost(1);
        edge7.setCost(10);
        edge8.setCost(4);
        edge9.setCost(25);
        edge10.setCost(6);
        edge11.setCost(7);
        // Create edges list and add values
        List<Edge> node1EdgeList = new List<Edge>();
        node1EdgeList.Add(edge1);
        node1EdgeList.Add(edge2);
        List<Edge> node2EdgeList = new List<Edge>();
        node2EdgeList.Add(edge9);
        List<Edge> node3EdgeList = new List<Edge>();
        node3EdgeList.Add(edge3);
        node3EdgeList.Add(edge4);
        List<Edge> node4EdgeList = new List<Edge>();
        node4EdgeList.Add(edge5);
        node4EdgeList.Add(edge6);
        List<Edge> node5EdgeList = new List<Edge>();
        node5EdgeList.Add(edge7);
        List<Edge> node6EdgeList = new List<Edge>();
        node6EdgeList.Add(edge8);
        List<Edge> node7EdgeList = new List<Edge>();
        node7EdgeList.Add(edge10);
        List<Edge> node8EdgeList = new List<Edge>();
        node8EdgeList.Add(edge11);
        // Add edges to nodes
        node1.SetEdges(node1EdgeList);
        node2.SetEdges(node2EdgeList);
        node3.SetEdges(node3EdgeList);
        node4.SetEdges(node4EdgeList);
        node5.SetEdges(node5EdgeList);
        node6.SetEdges(node6EdgeList);
        node7.SetEdges(node7EdgeList);
        node8.SetEdges(node8EdgeList);
        // Set holistic
        node1.SetHolistic(10);
        node2.SetHolistic(3);
        node3.SetHolistic(12);
        node4.SetHolistic(14);
        node5.SetHolistic(15);
        node6.SetHolistic(13);
        node7.SetHolistic(3);
        node8.SetHolistic(7);
    }
}