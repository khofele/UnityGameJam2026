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
    private bool m_isPlayerTurn = false;
    private Entity m_currentEntity = null;
    public bool PlayerCanCalculatePath = true;

    public List<Entity> LevelEntities
    {
        get { return m_levelEntities; }
    }

    private Pfadfindungsalgorithmus m_pfadfindungsalgorithmus;
    private Path m_path;

    public Path Path
    {
        get { return m_path; }
    }

    private void Start()
    {
        // Setup
        m_roundCounter = 1;
        //TODO Alle Entities in der szene finden und levelEntities (un-)geordnet hinzufügen??
        m_currentEntityIndex = 0;

        SubscribeEnemyEntities();

        RollOutActionPoints();
        NextEntityTurn();
    }

    private void SubscribeEnemyEntities()
    {
        foreach (Entity entity in m_levelEntities)
        {
            if (entity is EntityEnemy == true)
            {
                ((EntityEnemy)entity).OnEnemyKilled += HandleEnemyEntityKilled;
            }
        }
    }

    public void NextEntityTurn()
    {
        if (m_currentEntityIndex >= m_levelEntities.Count)
        {
            m_roundCounter++;
            m_currentEntityIndex = 0;

            NextRound();
        }

        m_currentEntity = m_levelEntities[m_currentEntityIndex];

        if (m_currentEntity is EntityPlayer == true)
        {
            m_isPlayerTurn = true;
        }
        else
        {
            m_isPlayerTurn = false;
        }

        m_currentEntityIndex++;
        HandleEntityTurnStart();

        // TODO entity needs to execute actions until every action point is used
    }

    private void NextRound()
    {
        if (m_roundCounter > m_maxRoundCounter)
        {
            foreach (Entity entity in m_levelEntities)
            {
                if (entity is EntityEnemy == true)
                {
                    if (entity.CurrentHealth > 0.0f)
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
            Debug.Log("---- Next round started --------------------------------------------------------------------------------------------------------");
            Debug.Log("Roundcounter " + m_roundCounter);
            RollOutActionPoints();
        }
    }

    private void RollOutActionPoints()
    {
        // TODO calculate action points for enemies?? TBD
        foreach(Entity entity in m_levelEntities)
        {
            entity.RollOutActionPoints();
            Debug.Log(entity.name + " rolled AP: " + entity.CurrentActionPoints);
            // TODO do something special if action points == lucky number --> für nächste 3 Runden +1 AP
        }
    }

    private void Update()
    {
        float fTimeDelta = Time.deltaTime;
        foreach (Entity entity in m_levelEntities)
        {
            entity.Tick(fTimeDelta);
        }
        //m_currentEntity.Tick(fTimeDelta);// nur wer dran ist bewegt sich
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

    // TODO Gamewon wenn alle Enemies tot auf Spielerseite --> abfragen ob nur noch spieler da
    // TODO Healthpotion

    private void HandleEntityTurnStart()
    {
        if (m_currentEntity.IsAlive == false)
        {
            NextEntityTurn();
            return;
        }

        Debug.Log("Current Entity " + m_currentEntity.name);

        // DEBUG
        //if (m_currentEntity is EntityPlayer)
        //{
        //    ((EntityPlayer)m_currentEntity).ReduceActionPoints(m_currentEntity.CurrentActionPoints);
        //}
        //if (m_currentEntity is EntityPlayer)
        //{
        //    ((EntityPlayer)m_currentEntity).SetActionPointsTo(400);
        //}
        // DEBUG

        if (m_currentEntity is EntityEnemy)
        {
            m_currentEntity.OnAllActionPointsUsed += HandleEntityTurnEnd;
            ((EntityEnemy)m_currentEntity).StartEnemyTurn();
        }
        else if (m_currentEntity.CurrentActionPoints == 0)
        {
            UIManager.Instance.EnableEndRoundButton();
        }
    }

    private void HandleEntityTurnEnd()
    {
        m_currentEntity.OnAllActionPointsUsed -= HandleEntityTurnEnd;

        NextEntityTurn();
    }

    private void HandleEnemyEntityKilled()
    {
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        foreach (Entity entity in m_levelEntities)
        {
            if (entity is EntityEnemy == true && entity.IsAlive == true)
            {
                return;
            }
        }

        GameManager.Instance.SetState(GameStates.GAMEWON);
    }

    public void HandlePlayerTurnEnd()
    {
        if (m_isPlayerTurn == false || m_currentEntity.CurrentActionPoints > 0)
        {
            return;
        }

        m_isPlayerTurn = false;
        UIManager.Instance.DisableEndRoundButton();

        NextEntityTurn();
    }
}
