using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EntityEnemy : Entity
{    
    [SerializeField] private EntityPlayer m_playerEntityTarget = null;
    [SerializeField] private EntityEnemyBehavior m_enemyBehavior = null;

    [SerializeField] private int m_renderTextureIndex = 0;// = 0 for GreenButts, = 1 for Ted
    public int RenderTextureIndex { get { return m_renderTextureIndex; } }

    public event Action<EntityEnemy> OnEnemyKilled;

    private bool m_standsStill = true;
    private bool m_doneAttacking = true;

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
            yield return new WaitUntil(() => m_standsStill == true);
            yield return new WaitUntil(() => m_doneAttacking == true);//macht anscheinend nichts
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
            OnEnemyKilled?.Invoke(this);
            AnimController.SetDeathTrigger();
        }
        AnimController.SetHitTrigger();
        UIManager.Instance.UpdateAttackUI(this, !m_isAlive);
    }

    public void StartEnemyTurn()
    {
        StartCoroutine(ExecuteEnemyTurn());
    }

    protected override void HandleMovementChange(bool _isMoving)
    {
        base.HandleMovementChange(_isMoving);
        m_standsStill = !_isMoving;
    }

    public void StartAttackTimer()
    {
        m_doneAttacking = false;
        StartCoroutine(AttackAnimationDelay());
    }
    private IEnumerator AttackAnimationDelay()
    {
        yield return null;
        float wait = m_animController.Animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(wait);
        yield return new WaitForSeconds(wait);
        m_doneAttacking = true;
    }
}
