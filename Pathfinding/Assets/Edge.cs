using UnityEngine;

public class Edge
{
    Node from;
    Node to;
    int cost;

    public Edge()
    {
       
    }

    public Edge(ref Node _from, ref Node _to)
    {
        from = _from;
        to = _to;
    }

    public void setEdges(ref Node _from, ref Node _to)
    {
        from = _from;
        to = _to;
    }

    public Node getNodeFrom()
    {
        return from;
    }

    public Node getNodeTo()
    {
        return to;
    }
}
