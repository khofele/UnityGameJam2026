using UnityEngine;

public class EntityEnemyDecision
{
    private EntityAction m_chosenAction = null;
    private Entity m_targetEntity = null;
    private Node m_actionDestinationNode = null;

    public EntityAction ChosenAction 
    {
        get { return m_chosenAction; } 
    }

    public Entity TargetEntity 
    { 
        get { return m_targetEntity; }
    }

    public Node ActionDestinationNode 
    { 
        get { return m_actionDestinationNode; }
    }

    public EntityEnemyDecision(EntityAction _chosenAction, Entity _targetEntity = null, Node _actionDestinationNode = default)
    {
        m_chosenAction = _chosenAction;
        m_targetEntity = _targetEntity;
        m_actionDestinationNode = _actionDestinationNode;
    }
}
