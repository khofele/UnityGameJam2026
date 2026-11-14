using System.Collections.Generic;
using UnityEngine;
public abstract class Pfadfindungsalgorithmus
{
    protected interface IComputeResult { Node PrevNode { get; } Node ThisNode { get; } }
    protected Node m_startNode;
    protected Node m_zielNode;
    protected Path m_path = new();
    protected abstract List<IComputeResult> Compute();
    public Path ComputePath(Node _ziel)
    {
        m_zielNode = _ziel;
        List<IComputeResult> result = Compute();
        int ind = result.FindIndex(n => n.ThisNode == m_zielNode);//prüft auf selbe Instanz
        if (ind == -1)
        {
            Debug.Log("Zielnode nicht gefunden");
            return m_path;//leer zurück
        }
        var itZiel = result[ind];
        m_path.AddNodeToPath(itZiel.ThisNode);
        while (itZiel.PrevNode != null)
        {
            itZiel = result[result.FindIndex(n => n.ThisNode == itZiel.PrevNode)];
            m_path.AddNodeToPath(itZiel.ThisNode);
        }
        m_path.ReversePath();
        //Debug.Log("Pfad berechnet");
        return m_path;
    }
}
