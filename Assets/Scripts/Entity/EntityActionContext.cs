using UnityEngine;

public class EntityActionContext
{
    private Entity m_executingEntity = null;
    private Entity m_targetEntity = null;
    private Node m_actionDestinationNode = null;

    public Entity ExecutingEntity 
    { 
        get { return m_executingEntity; } 
    }
    public Entity TargetEntity 
    { 
        get { return m_targetEntity; }
    }
    public Node ActionDestinationNode 
    { 
        get { return m_actionDestinationNode; }
    }

    public EntityActionContext(Entity _executingEnemy, Entity _targetEntity = null, Node _actionDestinationNode = default)
    {
        m_executingEntity = _executingEnemy;
        m_targetEntity = _targetEntity;
        m_actionDestinationNode = _actionDestinationNode;
    }
}