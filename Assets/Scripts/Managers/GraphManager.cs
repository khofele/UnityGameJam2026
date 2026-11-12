using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//public enum NodeZustand { free, blocked, lastPlayerPos}//auch für Bots?? -----> kann auch über einen bool steuern
public struct Edge
{
    public float Cost { get; /*init*/ private set; }
    public Node ToNode { get; private set; }

    public Edge(Node _toNode, float _cost)
    {
        ToNode = _toNode;
        Cost = _cost;
    }
}

public class Node
{
    public List<Edge> Edges { get; private set; }
    public Vector2 Position { get; private set; }
    //public NodeZustand Zustand { get => m_zustand; set => m_zustand = value; }//so statt getter und setter??
    //private NodeZustand m_zustand = NodeZustand.free;//graphmanager checkt ob node blockiert oder nicht

    public bool IsWalkable = true; //ersetzt enum NodeZustand

    public Node(Vector2 _position, int _expectedEdgecount = 4)//später vielleicht 8 wenn auch diagonal
    {
        Position = _position;
        Edges = new List<Edge>(_expectedEdgecount);
    }
    public void AddEdge(Node _to, float _cost = 1)
    {
        foreach (Edge e in Edges)
            if (e.ToNode == _to) return;

        Edges.Add(new Edge(_to, _cost));
        _to.Edges.Add(new Edge(this, _cost));
    }
}
public class Path
{
    private List<Node> m_nodes = new();//readonly -> nachschauen wie das funktioniert
    public int GetSize() { return m_nodes.Count; }
    public void AddNodeToPath(Node _node) { m_nodes.Add(_node); }
    public void ReversePath() { m_nodes.Reverse(); }
    public Node GetNodeAt(int ind) { return m_nodes[ind]; }

    public Path CutPath(int _lenght)
    {
        if (m_nodes.Count <= _lenght)
            return this;
        int anzElementsToRemove = m_nodes.Count - _lenght;
        m_nodes.RemoveRange(_lenght, anzElementsToRemove);
        return this;
    }
}
public class GraphManager : Manager<GraphManager>
{
    private Node[,] m_nodes;
    [SerializeField] private LayerMask m_blockingLayer;
    [SerializeField] private GameObject m_spielfeld;
    protected override void Awake()
    {
        base.Awake();

        int sizeX = Mathf.RoundToInt(m_spielfeld.transform.localScale.x * 10);
        int sizeY = Mathf.RoundToInt(m_spielfeld.transform.localScale.z * 10);
        GenerateNodeGraph(sizeX, sizeY);
    }
    public void GenerateNodeGraph(int _size_x, int _size_y)
    {
        m_nodes = new Node[_size_x, _size_y];
        for (int _x = 0; _x < _size_x; ++_x)
        {
            for (int _y = 0; _y < _size_y; ++_y)
            {
                AddNode(_x, _y);
            }
        }
        //edges erst wenn alle nodes existieren
        for (int _x = 0; _x < _size_x - 1; ++_x)
        {
            for (int _y = 0; _y < _size_y - 1; ++_y)
            {
                if (_x != 0)
                {
                    m_nodes[_x, _y].AddEdge(m_nodes[_x - 1, _y]);//ggfs später edge cost anpassen, CalculateEdgeCost()
                }
                if (_y != 0)
                {
                    m_nodes[_x, _y].AddEdge(m_nodes[_x, _y - 1]);
                }
                if (_x != _size_x)
                {
                    m_nodes[_x, _y].AddEdge(m_nodes[_x + 1, _y]);
                }
                if (_y != _size_y)
                {
                    m_nodes[_x, _y].AddEdge(m_nodes[_x, _y + 1]);
                }
            }
        }
    }
    private void AddNode(int _x, int _y)
    {
        Node _newNode = new(new Vector2(_x, _y));
        m_nodes[_x, _y] = _newNode;

        //node nicht walkable setzten, falls objekt drüber
        Vector3 von = new(_x, 0f, _y);
        if (Physics.Raycast(von, Vector3.up, 0.1f, m_blockingLayer))
        {
            _newNode.IsWalkable = false;
        }
    }

    //wird aufgerufen am rundenende von Spieler und Enemy.
    //Spieler und Enemy merken sich immer current position (also transform.position) und last position (das zuletzt benutzte Node/Feld)
    //So kann die zuletzt 'besuchte' Node gesperrt/entsperrt werden.
    public void SetNodeZustand(int _x, int _y, bool _IsWalkable) //NodeZustand _zustand
    {
        //m_nodes[_x, _y].Zustand = _zustand;//so oder so
        m_nodes[_x, _y].IsWalkable = _IsWalkable;
    }
    public void SetNodeZustand(Vector2 _coords, bool _IsWalkable) { SetNodeZustand(Mathf.RoundToInt(_coords.x), Mathf.RoundToInt(_coords.y), _IsWalkable); }
    public Node FindClosestNode(Vector3 _pos)
    {
        return m_nodes[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.z)];//war _pos.y und ging deshalb nicht
    }
    public Node GetNodeAt(int _x, int _y)
    {
        return m_nodes[_x, _y];
    }
    public bool CanMoveToPosition(Vector3 _pos)
    {
        return FindClosestNode(_pos).IsWalkable;
    }
}