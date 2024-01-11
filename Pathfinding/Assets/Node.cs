using UnityEngine;
using System.Collections.Generic;

public class Node
{
    private Vector2 pos;
    List<Edge> edges;
    int holisitc;

    public Node(Vector2 _pos, List<Edge> _edges, int _holistic)
    {
        pos = _pos;
        edges = _edges;
        holisitc = _holistic;
    }
}
