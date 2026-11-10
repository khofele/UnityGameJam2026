using UnityEngine;
using System.Collections.Generic;
public class Dijkstra : Pfadfindungsalgorithmus
{
    private struct DijkstraNode : IComputeResult
    {
        public float AccumulatedCosts;
        public Node PrevNode { get; set; }
        public Node ThisNode { get; set; }
        public DijkstraNode(Node _n, float _c)
        {
            AccumulatedCosts = _c; ThisNode = _n; PrevNode = null;
        }
    };

    private List<DijkstraNode> m_openList = new();
    private List<IComputeResult> m_closedList = new();
    public Dijkstra(Node _startNode) { m_startNode = _startNode; }
    protected override List<IComputeResult> Compute()
    {
        m_openList.Clear();
        m_closedList.Clear();

        m_openList.Add(new DijkstraNode(m_startNode, 0f));
        while (m_openList.Count > 0)
        {
            float cost = m_openList[0].AccumulatedCosts;
            int ind = 0;
            for (int i = 1; i < m_openList.Count; ++i)
            {
                if (m_openList[i].AccumulatedCosts < cost)
                {
                    cost = m_openList[i].AccumulatedCosts;
                    ind = i;
                }
            }
            DijkstraNode k = m_openList[ind];
            m_openList.RemoveAt(ind);
            m_closedList.Add(k);
            foreach (Edge edge in k.ThisNode.Edges)
            {
                if (edge.ToNode.IsWalkable == false) continue; //node ignorieren, wenn man sie nicht betreten darf
                if (m_closedList.Exists(n => n.ThisNode == edge.ToNode)) continue;
                float tempCost = k.AccumulatedCosts + edge.Cost;
                int indexOpen = m_openList.FindIndex(n => n.ThisNode == edge.ToNode);
                if (indexOpen != -1)
                {
                    DijkstraNode updateNode = m_openList[indexOpen];
                    if (tempCost < updateNode.AccumulatedCosts)
                    {
                        updateNode.AccumulatedCosts = tempCost;
                        updateNode.PrevNode = k.ThisNode;
                        m_openList[indexOpen] = updateNode;
                    }
                }
                else
                {
                    DijkstraNode nk = new(edge.ToNode, tempCost) { PrevNode = k.ThisNode };
                    m_openList.Add(nk);
                }
            }
        }
        return m_closedList;
    }
}
