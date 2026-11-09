using UnityEngine;
using System.Collections.Generic;
public class A_Stern : Pfadfindungsalgorithmus
{
    private struct A_SternNode : IComputeResult
    {
        public float Heuristic;
        public float AccumulatedCosts;
        public float F;
        public Node PrevNode { get; set; }
        public Node ThisNode { get; set; }
        public A_SternNode(Node n, float _distanz, float _costs, float _f)
        {
            Heuristic = _distanz;//distanz this <-> target
            AccumulatedCosts = _costs;//zurückgelegter weg
            F = _f;//Schätzfunktion ---- F = AccumulatedCosts + Heuristic
            ThisNode = n; 
            PrevNode = null;
        }
    };
    private List<A_SternNode> m_openList = new();
    private List<IComputeResult> m_closedList = new();
    public A_Stern(Node _startNode) { m_startNode = _startNode; }
    protected override List<IComputeResult> Compute()
    {
        m_openList.Clear();
        m_closedList.Clear();

        float h_1 = (m_zielNode.Position - m_startNode.Position).magnitude;
        m_openList.Add(new A_SternNode(m_startNode, h_1, 0f, h_1));
        while (m_openList.Count > 0)
        {
            float cost = m_openList[0].F;
            int ind = 0;
            for (int i = 1; i < m_openList.Count; ++i)
            {
                if (m_openList[i].F < cost)
                {
                    cost = m_openList[i].F;
                    ind = i;
                }
            }
            A_SternNode k = m_openList[ind];
            m_openList.RemoveAt(ind);
            m_closedList.Add(k);
            if (k.ThisNode == m_zielNode) break;
            foreach (Edge edge in k.ThisNode.Edges)
            {
                if (edge.ToNode.IsWalkable == false) continue;//nur wenn man da hingehen kann
                if (m_closedList.Exists(n => n.ThisNode == edge.ToNode)) continue;
                float tempCost = k.AccumulatedCosts + edge.Cost;
                float tempHeur = (m_zielNode.Position - edge.ToNode.Position).magnitude;
                float tempF = tempCost + tempHeur;
                int indexOpen = m_openList.FindIndex(n => n.ThisNode == edge.ToNode);
                if (indexOpen != -1)
                {
                    A_SternNode updateNode = m_openList[indexOpen];
                    if (tempCost < updateNode.AccumulatedCosts)//tempF < updateNode.F
                    {
                        updateNode.AccumulatedCosts = tempCost;
                        updateNode.Heuristic = tempHeur;
                        updateNode.F = tempF;
                        updateNode.PrevNode = k.ThisNode;
                        m_openList[indexOpen] = updateNode;
                    }
                }
                else
                {
                    A_SternNode nk = new(edge.ToNode, tempHeur, tempCost, tempF) { PrevNode = k.ThisNode };
                    m_openList.Add(nk);
                }
            }
        }
            return m_closedList;
    }
}
