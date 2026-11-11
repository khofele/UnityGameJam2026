using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    [Header("Level Setup")]
    [SerializeField] private List<Entity> m_levelEntities = null;
    [SerializeField] private int m_maxRoundCounter = 0;
    [SerializeField] private int m_luckyNumber = 5; // TODO only temporary serializefield!! Choose lucky number in main menu or somewhere else before start of the game

    private int m_roundCounter = 0;
    private int m_currentEntityIndex = 0;

    public List<Entity> LevelEntities
    {
        get { return m_levelEntities; }
    }

    private Pfadfindungsalgorithmus m_pfadfindungsalgorithmus;
    private Path m_path;

    private void Start()
    {
        // Setup
        m_roundCounter = 0;
        //TODO Alle Entities in der szene finden und levelEntities (un-)geordnet hinzufügen??
        m_currentEntityIndex = 0;

        RollOutActionPoints();
        NextEntityTurn();
    }

    public void NextEntityTurn() // TODO Call on button press in level, was private
    {
        if(m_currentEntityIndex >= m_levelEntities.Count)
        {
            m_roundCounter++;
            m_currentEntityIndex = 0;

            NextRound();
        }

        // TODO entity needs to execute actions until every action point is used

        Debug.Log("Current Entity " + m_currentEntityIndex);

        m_currentEntityIndex++;
    }

    private void NextRound()
    {
        if(m_roundCounter > m_maxRoundCounter)
        {
            foreach(Entity entity in m_levelEntities)
            {
                if(entity is EntityEnemy == true)
                {
                    if(entity.CurrentHealth > 0.0f)
                    {
                        GameManager.Instance.SetState(GameStates.GAMEOVER);
                        return;
                    }
                }
            }

            GameManager.Instance.SetState(GameStates.GAMEWON); // TODO trigger level change here (if we have multiple levels)
        }
        else
        {
            Debug.Log("Next round started");

            RollOutActionPoints();
        }
    }

    private void RollOutActionPoints()
    {
        // TODO calculate action points for enemies?? TBD
        foreach(Entity entity in m_levelEntities)
        {
            entity.RollOutActionPoints();
            // TODO do something special if action points == lucky number
        }
    }

    private void Update()
    {
        float fTimeDelta = Time.deltaTime;
        foreach (Entity entity in m_levelEntities)
        {
            entity.Tick(fTimeDelta);
        }
    }
    public void CalculateEntityMovePath(Entity _entity, Node _target)//weiß nicht wie gegner KI entscheiden wo hin
    {
        Debug.Log("Berechne Pfad von " + GraphManager.Instance.FindClosestNode(_entity.transform.position).Position + " nach " + _target.Position);
        m_pfadfindungsalgorithmus = new A_Stern(GraphManager.Instance.FindClosestNode(_entity.transform.position));
        m_path = m_pfadfindungsalgorithmus.ComputePath(_target);
        Debug.Log("Pfad mit A* berechnet! Länge: " +  m_path.GetSize());
        if (_entity is EntityPlayer)
        {
            UIManager.Instance.HandleGroundClicked(m_path);
        }
        else
        {
            //TODO Logik für KI entscheidung
            ConfirmEntityMove(_entity);
        }
    }
    public void ConfirmEntityMove(Entity _entity)
    {
        _entity.SetSteeringBehaviour(eSteeringBehaviour.GrapplinghooksKin, m_path);
    }
}
