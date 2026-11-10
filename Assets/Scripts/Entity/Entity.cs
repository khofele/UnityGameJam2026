using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private EntityProperties m_entityProperties = null;
    private SteeringBehaviourKin m_steering;
    private eSteeringBehaviour m_currentBehaviour;
    //Runde beenden -> GraphManager SetNodeStatus (m_currentNodePos, false)
    private Vector2 m_currentNodePos;
    private Vector2 m_prevNodePos;
    
    protected Kinematics m_kinematics;
    protected float m_currentHealth = 0.0f;
    protected int m_currentActionsPoints = 0;
    protected bool m_isAlive = true;
    private bool m_isEntitiesTurn = false; // TODO im Levelmanager setzen
        // TODO Nodegraph resetten nach Ende der Runde

    public event Action OnAllActionPointsUsed;

    public float CurrentHealth
    {
        get { return m_currentHealth; }
    }

    public int CurrentActionPoints
    {
        get { return m_currentActionsPoints; }
    }

    public bool IsAlive
    {
        get { return m_isAlive; }
    }

    public EntityProperties EntityProperties 
    { 
        get { return m_entityProperties; }
    }

    protected virtual void Awake()
    {
        m_currentHealth = m_entityProperties.MaxHealth;
        m_currentActionsPoints = 0;

        //SteeringBehaviour
        m_kinematics = new();
        m_kinematics.Position = transform.position;
        m_kinematics.Rotation = transform.eulerAngles.y * Mathf.Deg2Rad;
    }

    protected void TriggerOnAllActionPointsUsedEvent()
    {
        OnAllActionPointsUsed?.Invoke();
    }

    public abstract void TakeDamage(float _damageValue);

    public abstract void ReduceActionPoints(int _actionPoints);

    public void RollOutActionPoints()
    {
        int actionPoints = 0;

        actionPoints = UnityEngine.Random.Range(m_entityProperties.MinActionPoints, m_entityProperties.MaxActionPoints);

        m_currentActionsPoints = actionPoints;
    }

    public int GetTileDistance(Entity _targetEntity)
    {
        // TODO needed?
        return 1;
    }

    public bool CanExecuteAction(EntityAction _entityAction, EntityActionContext _actionContext)
    {
        if (_entityAction != null && m_currentActionsPoints >= _entityAction.ActionCost && _entityAction.CanExecuteAction(_actionContext))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ExecuteAction(EntityAction _chosenAction, EntityActionContext _actionContext)
    {
        if (CanExecuteAction(_chosenAction, _actionContext) == false)
        {
            return false;
        }

        Debug.Log("Executing " + _chosenAction.name + " from " + gameObject.name);

        _chosenAction.ExecuteAction(_actionContext);

        ReduceActionPoints(_chosenAction.ActionCost);

        return true;
    }

    public void TraversePath(Path path)   //doch lieber mit steering behaviour
    {
        //Goto node, if reached, goto next node, if done stop else repeat
    }

    protected virtual void OnTick(float fTimeDelta) { }

    public void Tick(float fTimeDelta)
    {
        m_steering?.Tick(fTimeDelta);
        transform.SetPositionAndRotation(m_kinematics.Position, Quaternion.Euler(0f, m_kinematics.Rotation * Mathf.Rad2Deg, 0f));
        OnTick(fTimeDelta);
    }

    public void SetSteeringBehaviour(eSteeringBehaviour _newBehaviour, Path _path = null)
    {
        m_currentBehaviour = _newBehaviour;
        Debug.Log("Setting Steering Behaviour to " + m_currentBehaviour);

        switch (m_currentBehaviour)
        {
            case eSteeringBehaviour.None:
                break;
            case eSteeringBehaviour.GrapplinghooksKin:
                if (_path != null)
                {
                    m_steering = new SBgrapplinghooksKin(_path); //oder statt new einen start/stop trigger setzen??
                    m_currentNodePos = _path.GetNodeAt(_path.GetSize()-1).Position;
                    m_prevNodePos = _path.GetNodeAt(_path.GetSize() - 2).Position;
                }
                else Debug.Log("Pfad ist null");
                break;
        }
        m_steering?.SetKinematic(m_kinematics);
        //Debug.Log(m_steering);
    }
}

public enum eSteeringBehaviour { None, GrapplinghooksKin}