using System;
using UnityEngine;

public abstract class EntityAction : ScriptableObject
{
    [SerializeField] private string m_actionName = "";
    [SerializeField] private int m_actionPriority = 0;
    [SerializeField] protected int m_actionCost = 1;

    public string ActionName
    {
        get { return m_actionName; }
    }

    public int ActionPriority
    {
        get { return m_actionPriority; }
    }

    public int ActionCost
    {
        get { return m_actionCost; }
    }

    public abstract bool CanExecuteAction(EntityActionContext _actionContext);
    public abstract void ExecuteAction(EntityActionContext _actionContext);
}
