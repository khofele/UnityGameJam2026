using System;
using UnityEngine;
[RequireComponent (typeof(EntityAnimController))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] private EntityProperties m_entityProperties = null;

    protected float m_currentHealth = 0.0f;
    protected int m_currentActionsPoints = 0;
    protected bool m_isAlive = true;

    private EntityAnimController m_animController;
    private bool m_isEntitiesTurn = false; // TODO im Levelmanager setzen
                                           // TODO Nodegraph resetten nach Ende der Runde

    public event Action OnAllActionPointsUsed;

    public float CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    public bool IsAlive
    {
        get { return m_isAlive; }
    }

    public int CurrentActionPoints
    {
        get { return m_currentActionsPoints; }
    }

    public EntityProperties EntityProperties
    {
        get { return m_entityProperties; }
    }

    private void Awake()
    {
        m_animController = GetComponent<EntityAnimController>();

        // Entity Setup
        m_currentHealth = m_entityProperties.MaxHealth;
        m_currentActionsPoints = 0;

        //SteeringBehaviour
        m_kinematics = new();
        m_kinematics.Position = transform.position;
        m_kinematics.Rotation = transform.eulerAngles.y * Mathf.Deg2Rad;

        m_currentNodePos = gameObject.transform.position;
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

    //public void TraversePath(Path path)   //doch lieber mit steering behaviour
    //{
    //    //Goto node, if reached, goto next node, if done stop else repeat
    //}
    private SteeringBehaviourKin m_steering;
    protected Kinematics m_kinematics;
    private eSteeringBehaviour m_currentSteeringBehaviour;

    //Runde beenden -> GraphManager SetNodeStatus (m_currentNodePos, false)
    private Vector2 m_currentNodePos;
    private Vector2 m_prevNodePos;

    public Vector2 CurrentNodePos
    {
        get { return m_currentNodePos; }
    }

    //public bool IsMoving { get; private set; }

    //protected virtual void OnTick(float fTimeDelta) { }
    public void Tick(float fTimeDelta)
    {
        m_steering?.Tick(fTimeDelta);
        transform.SetPositionAndRotation(m_kinematics.Position, Quaternion.Euler(0f, m_kinematics.Rotation * Mathf.Rad2Deg, 0f));
    }
    //public bool GetIsMoving()//brauche nicht mehr aber als Bsp für null-coalescing-operator
    //{
    //        return m_steering?.IsMoving ?? false;
    //}
    public void SetSteeringBehaviour(eSteeringBehaviour _newBehaviour, Path _path = null)
    {
        m_currentSteeringBehaviour = _newBehaviour;
        Debug.Log("Setting Steering Behaviour to " + m_currentSteeringBehaviour);

        switch (m_currentSteeringBehaviour)
        {
            case eSteeringBehaviour.None:
                break;
            case eSteeringBehaviour.GrapplinghooksKin:
                if (_path != null)
                {
                    if(_path.GetSize()>m_currentActionsPoints + 1)
                        _path = _path.CutPath(m_currentActionsPoints);
                    m_steering = new SBgrapplinghooksKin(_path); //oder statt new einen start/stop trigger setzen??
                    m_steering.OnMovementStateChanged += HandleMovementChange;
                    if (_path.GetSize() > 1)
                    {
                        if (_path.GetSize() == 2)
                            m_prevNodePos = m_currentNodePos;
                        else
                            m_prevNodePos = _path.GetNodeAt(_path.GetSize() - 2).Position;
                        m_currentNodePos = _path.GetNodeAt(_path.GetSize() - 1).Position;//path hat länge 0
                    }
                }
                else Debug.Log("Pfad ist null");
                break;
        }
        m_steering?.SetKinematic(m_kinematics);
        //Debug.Log(m_steering);
    }
    protected virtual void HandleMovementChange(bool _isMoving)
    {
        m_animController.SetMovementParameter(_isMoving);
        if(_isMoving == false) m_steering.OnMovementStateChanged -= HandleMovementChange;
    }
    private void OnDisable()
    {
        if (m_steering != null)
            m_steering.OnMovementStateChanged -= HandleMovementChange;
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
}
public enum eSteeringBehaviour { None, GrapplinghooksKin}