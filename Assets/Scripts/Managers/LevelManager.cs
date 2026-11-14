using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    [Header("Level Setup")]
    [SerializeField] private List<Entity> m_levelEntities = null;
    [SerializeField] private int m_maxRoundCounter = 0;
    public int MaxRoundCounter {  get { return m_maxRoundCounter; }}
    [SerializeField] private int m_luckyNumber = 5; // TODO only temporary serializefield!! Choose lucky number in main menu or somewhere else before start of the game

    private int m_roundCounter = 0;
    private int m_currentEntityIndex = 0;
    private bool m_isPlayerTurn = false;
    private Entity m_currentEntity = null;
    public bool PlayerCanCalculatePath = true;

    [SerializeField] private float m_maxPlayerAttackReach = 1.6f;
    [SerializeField] private float m_attackReach = 1.2f;
    public float MaxPlayerAttackReach { get { return m_maxPlayerAttackReach; } }
    public float AttackReach {  get { return m_attackReach; } }
    [SerializeField] private int m_minPlayerDamage = 10;
    [SerializeField] private int m_maxPlayerDamage = 20;
    [SerializeField] private int m_minEnemyDamage = 5;
    [SerializeField] private int m_maxEnemyDamage = 15;

    [SerializeField] private float m_attackAnimationDelay = 0.1f;

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
    private int m_luckyIncreaseCounter = 0;
    [SerializeField] private int m_luckyNumberAPIncrease = 1;//amount of extra AP the player gets for three rounds after a lucky number roll
    [SerializeField] private int m_luckyIncreaseRoundTimer = 3;//duration the lucky AP increase lasts
    private void Start()
    {
        // Setup
        m_roundCounter = 1;
        //TODO Alle Entities in der szene finden und levelEntities (un-)geordnet hinzufügen??
        m_currentEntityIndex = 0;

        SubscribeEnemyEntities();

        RollOutActionPoints();
        NextEntityTurn();

        if (GameManager.Instance != null)
            m_luckyNumber = GameManager.Instance.LuckyNumber;//mit entity properties und MainMenuUI abgleichen.
        else
            m_luckyNumber = Random.Range(1, 14);//falls Game Manager nicht gefunden, abgleichen
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
            UIManager.Instance.ShowYourTurnImg();
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
        if (m_roundCounter > m_maxRoundCounter && GameManager.Instance.CurrentGameState != GameStates.GAMEWON)
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
            UIManager.Instance.UpdateRoundCounter(m_roundCounter,m_maxRoundCounter);
            RollOutActionPoints();
        }
    }

    private void RollOutActionPoints()
    {
        if (m_luckyIncreaseCounter > 0) m_luckyIncreaseCounter--;
        // TODO calculate action points for enemies?? TBD
        foreach(Entity entity in m_levelEntities)
        {
            entity.RollOutActionPoints();
            Debug.Log(entity.name + " rolled AP: " + entity.CurrentActionPoints);
            // TODO do something special if action points == lucky number --> für nächste 3 Runden +1 AP
            if (entity is EntityPlayer)
            {
                UIManager.Instance.UpdateCurrentAPText(entity.CurrentActionPoints);
                UIManager.Instance.SetPlayerMoveButtonInteractable(true);//Pfuschen ist immer gut
                if (entity.CurrentActionPoints == m_luckyNumber)
                {
                    m_luckyIncreaseCounter = m_luckyIncreaseRoundTimer;
                }
                if (m_luckyIncreaseCounter > 0)
                    entity.SetActionPointsTo(entity.CurrentActionPoints + m_luckyNumberAPIncrease);
            }
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
        //auch Entity TurnTowardsTargetAnpassen
    }
    public void CalculateEntityMovePath(Entity _entity, Node _target)//weiß nicht wie gegner KI entscheiden wo hin
    {
        //Debug.Log("Berechne Pfad von " + GraphManager.Instance.FindClosestNode(_entity.transform.position).Position + " nach " + _target.Position);
        m_pfadfindungsalgorithmus = new A_Stern(GraphManager.Instance.FindClosestNode(_entity.transform.position));
        m_path = m_pfadfindungsalgorithmus.ComputePath(_target);
        //Debug.Log("Pfad mit A* berechnet! Länge: " +  m_path.GetSize());
        if (_entity is EntityPlayer)
        {
            UIManager.Instance.HandleGroundClicked(m_path);
        }
        else
        {
            //TODO Logik für KI entscheidung
            //ConfirmEntityMove(_entity);
        }
    }
    public void ConfirmEntityMove(Entity _entity)
    {
        GraphManager.Instance.SetNodeZustand(_entity.CurrentNodePos, true);
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

        foreach (Entity entity in m_levelEntities)//called jetzt alle bei jedem rundenstart, lieber einmal callen bei eigenem rundenende
        {
            if (entity is EntityPlayer && m_isPlayerTurn == false) 
                GraphManager.Instance.SetNodeZustand(entity.CurrentNodePos, true);//Bots finden spieler sonst nicht
            else
                GraphManager.Instance.SetNodeZustand(entity.CurrentNodePos, false);//change back before moving
            GraphManager.Instance.SetNodeZustand(entity.PrevNodePos, true); //prev node pos nur in der eigenen runde relevant
        }

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

    private void HandleEnemyEntityKilled(EntityEnemy _enemy)
    {
        m_levelEntities.Remove(_enemy);
        StartCoroutine(EnemyDeath(_enemy));
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

    public int RollDamage(bool _isPlayer)
    {
        if (_isPlayer)
            return Random.Range(m_minPlayerDamage, m_maxPlayerDamage + 1);
        else
            return Random.Range(m_minEnemyDamage, m_maxEnemyDamage + 1);
    }

    public void ConfirmEntityAttack(Entity _attackingEntity, Entity _attackedEntity, float _damage)
    {
        _attackingEntity.TurnTowardsTarget(_attackedEntity.CurrentNodePos);
        _attackedEntity.TurnTowardsTarget(_attackingEntity.CurrentNodePos);//beide schauen sich an?? oder lieber nicht
        _attackingEntity.AnimController.SetAttackTrigger();
        if (_attackingEntity.TryGetComponent(out EntityEnemy _enemy)) _enemy.StartAttackTimer(); 
        StartCoroutine(AttackAnimationDelay(_attackedEntity, _damage));
    }

    private IEnumerator AttackAnimationDelay(Entity _attackedEntity, float _damage)
    {
        yield return new WaitForSeconds(m_attackAnimationDelay);
        _attackedEntity.TakeDamage(_damage);
        if (_attackedEntity.TryGetComponent(out EntityPlayer _player)) UIManager.Instance.UpdatePlayerHPDisplay(_player.CurrentHealth);
    }
    private IEnumerator EnemyDeath(EntityEnemy _enemy)
    {
        yield return new WaitForSeconds(5f);
        GraphManager.Instance.SetNodeZustand(_enemy.CurrentNodePos, true);
        UIManager.Instance.ResetEACs();
        //Destroy(_enemy.gameObject);//oder lieber doch wo anders hin schieben
    }
}
