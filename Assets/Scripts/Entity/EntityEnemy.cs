using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EntityEnemy : Entity
{    
    [SerializeField] private EntityPlayer m_playerEntityTarget = null;
    [SerializeField] private EntityEnemyBehavior m_enemyBehavior = null;

    public event Action OnEnemyKilled;

    private IEnumerator ExecuteEnemyTurn()
    {
        while (m_currentActionsPoints > 0)
        {
            EntityEnemyDecision decision = m_enemyBehavior.MakeActionDecision(this, m_playerEntityTarget);

            if (decision == null)
            {
                break;
            }

            EntityActionContext actionContext = new EntityActionContext(this, decision.TargetEntity, decision.ActionDestinationNode);

            bool isActionSuccessful = ExecuteAction(decision.ChosenAction, actionContext); // TODO wenn Moveaction muss successful erst kommen wenn angekommen

            if (isActionSuccessful == false)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        Debug.Log(gameObject.name + " finished");
        TriggerOnAllActionPointsUsedEvent();
    }

    public override void ReduceActionPoints(int _actionPoints) // TODO evtl. Methode wieder zusammenlegen
    {
        if ((m_currentActionsPoints - _actionPoints) >= 0)
        {
            m_currentActionsPoints -= _actionPoints;
        }

        Debug.Log(gameObject.name + " Remaining AP " + m_currentActionsPoints);
    }

    public override void TakeDamage(float _damageValue)
    {
        m_currentHealth -= _damageValue;

        if (m_currentHealth <= 0.0f)
        {
            m_isAlive = false;
            OnEnemyKilled?.Invoke();
        }
    }

    public void StartEnemyTurn()
    {
        StartCoroutine(ExecuteEnemyTurn());
    }
}
