using UnityEngine;

public abstract class EntityAction : ScriptableObject
{
    [SerializeField] private string m_actionName = "";
    [SerializeField] private int m_actionCost = 1;

    public string ActionName
    {
        get { return m_actionName; }
    }

    public int ActionCost
    {
        get { return m_actionCost; }
    }
}
