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
    }

    // TODO call on damage taken
    public abstract void HandleHealthChange();

    public int RollOutActionPoints()
    {
        int actionPoints = 0;

        actionPoints = Random.Range(m_entityProperties.MinActionPoints, m_entityProperties.MaxActionPoints);

        return actionPoints;
    }
}
