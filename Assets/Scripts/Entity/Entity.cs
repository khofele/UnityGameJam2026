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
    private Kinematics m_kinematics;
    private SteeringBehaviours m_currentBehaviour;
    public void Tick(float fTimeDelta)
    {
        transform.position = m_kinematics.Position;
        transform.rotation = Quaternion.Euler(0f, m_kinematics.Rotation * Mathf.Rad2Deg, 0f);
    }
    public void SetSteeringBehaviour(SteeringBehaviours newBehaviour, Path path = null)
    {
        m_currentBehaviour = newBehaviour;
        Debug.Log("Setting Steering Behaviour to " + m_currentBehaviour);

        switch (m_currentBehaviour)
        {
            case SteeringBehaviours.None:
                break;
            case SteeringBehaviours.GrapplinghooksKin:
                if (path != null)
                    m_steering = new SBgrapplinghooksKin(path); //oder statt new einen start/stop trigger setzen
                else Debug.Log("Pfad ist null");
                break;
        }
        m_steering?.SetKinematic(m_kinematics);
        //Debug.Log(m_steering);
    }
}
public enum SteeringBehaviours { None, GrapplinghooksKin}