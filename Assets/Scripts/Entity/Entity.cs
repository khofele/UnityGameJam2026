using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private EntityProperties m_entityProperties = null;

    protected float m_currentHealth = 0.0f;
    protected int m_currentActionsPoints = 0;

    public float CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    protected void Start()
    {
        // Entity Setup
        m_currentHealth = m_entityProperties.MaxHealth;
        m_currentActionsPoints = 0;

        //SteeringBehaviour
        m_kinematics = new();
        m_kinematics.Position = transform.position;
        m_kinematics.Rotation = transform.eulerAngles.y * Mathf.Deg2Rad;
    }

    // TODO call on damage taken
    public abstract void HandleHealthChange();

    public int RollOutActionPoints()
    {
        int actionPoints = 0;

        actionPoints = Random.Range(m_entityProperties.MinActionPoints, m_entityProperties.MaxActionPoints);

        return actionPoints;
    }

    //public void TraversePath(Path path)   //doch lieber mit steering behaviour
    //{
    //    //Goto node, if reached, goto next node, if done stop else repeat
    //}
    private SteeringBehaviourKin m_steering;
    protected Kinematics m_kinematics;
    private eSteeringBehaviour m_currentBehaviour;

    //Runde beenden -> GraphManager SetNodeStatus (m_currentNodePos, false)
    private Vector2 m_currentNodePos;
    private Vector2 m_prevNodePos;

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