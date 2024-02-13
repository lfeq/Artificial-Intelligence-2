using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    public static List<Node> Dijkstra(Node[] t_nodes, Node t_startNode, Node t_targetNode) {
        List<Node> visited = new List<Node>();
        List<Node> unvisited = new List<Node>();
        Node tempNode;
        // Set distances to infinity except for the starter node
        for (int i = 0; i < t_nodes.Length; i++) {
            unvisited.Add(t_nodes[i]);
            if (t_nodes[i] == t_startNode) {
                t_nodes[i].shortestDistance = 0;
            } else {
                t_nodes[i].shortestDistance = float.MaxValue;
            }
            t_nodes[i].previousNode = null;
        }
        while (visited.Count != t_nodes.Length) {
            unvisited = SortList(unvisited);
            tempNode = unvisited[0];
            // Finish while loop because we found the target node
            if (tempNode == t_targetNode) {
                visited.Add(tempNode);
                unvisited.Remove(tempNode);
                unvisited.TrimExcess();
                break;
            }
            float[] edgesWeights = tempNode.GetEdgesWeights();
            Node[] neighbourNodes = tempNode.GetNeighbourNodes();
            // Set shortest distance and previous node for adyacent nodes of the current node
            for (int i = 0; i < neighbourNodes.Length; i++) {
                if (visited.Contains(tempNode)) {
                    break;
                }
                float newDistance = tempNode.shortestDistance + edgesWeights[i];
                if (newDistance < neighbourNodes[i].shortestDistance) {
                    neighbourNodes[i].shortestDistance = newDistance;
                    neighbourNodes[i].previousNode = tempNode;
                }
            }
            visited.Add(tempNode);
            unvisited.Remove(tempNode);
            unvisited.TrimExcess();
        }
        List<Node> path = new List<Node>();
        tempNode = visited[visited.Count - 1];
        while (tempNode.previousNode != null) {
            path.Add(tempNode);
            tempNode = tempNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    public static List<Node> AStar(Node[] t_nodes, Node t_startNode, Node t_targetNode) {
        List<Node> nodesToVisit = new List<Node>();
        List<Node> visited = new List<Node>();
        Node tempNode;
        for (int i = 0; i < t_nodes.Length; i++) {
            t_nodes[i].distanceToTarget = Vector3.Distance(t_nodes[i].transform.position, t_targetNode.transform.position);
            t_nodes[i].shortestDistance = float.MaxValue;
            t_nodes[i].previousNode = null;
            if (t_nodes[i] == t_startNode) {
                t_nodes[i].shortestDistance = 0;
            }
        }
        nodesToVisit.Add(t_startNode);
        while (nodesToVisit.Count != 0) {
            tempNode = nodesToVisit[0];
            if (tempNode == t_targetNode) {
                visited.Add(tempNode);
                break;
            }
            float[] edgesWeights = tempNode.GetEdgesWeights();
            Node[] neighbourNodes = tempNode.GetNeighbourNodes();
            for (int i = 0; i < neighbourNodes.Length; i++) {
                if (visited.Contains(tempNode)) {
                    break;
                }
                float newDistance = tempNode.shortestDistance + edgesWeights[i];
                if (newDistance < neighbourNodes[i].shortestDistance) {
                    neighbourNodes[i].shortestDistance = newDistance;
                    neighbourNodes[i].heuristic = edgesWeights[i] + neighbourNodes[i].distanceToTarget;
                    neighbourNodes[i].previousNode = tempNode;
                }
                nodesToVisit.Add(neighbourNodes[i]);
            }
            visited.Add(tempNode);
            nodesToVisit.Remove(tempNode);
            nodesToVisit.TrimExcess();
            nodesToVisit = SortListHeuristic(nodesToVisit);
        }
        List<Node> path = new List<Node>();
        tempNode = visited[visited.Count - 1];
        while (tempNode.previousNode != null) {
            path.Add(tempNode);
            tempNode = tempNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private static List<Node> SortList(List<Node> t_listToSort) {
        t_listToSort.Sort(delegate (Node x, Node y) { return x.shortestDistance.CompareTo(y.shortestDistance); });
        return t_listToSort;
    }

    private static List<Node> SortListHeuristic(List<Node> t_listToSort) {
        t_listToSort.Sort(delegate (Node x, Node y) { return x.heuristic.CompareTo(y.heuristic); });
        return t_listToSort;
    }
}