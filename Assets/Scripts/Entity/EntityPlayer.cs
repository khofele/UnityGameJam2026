using UnityEngine;

public class EntityPlayer : Entity
{
    public bool IsMoving { get; private set; }

    protected override void OnTick(float fTimeDelta)
    {
        if (m_kinematics.MoveVelo.sqrMagnitude < 0.1f)
            IsMoving = false;
        else
            IsMoving = true;
    }

    public override void ReduceActionPoints(int _actionPoints)
    {
        if ((m_currentActionsPoints - _actionPoints) >= 0)
        {
            m_currentActionsPoints -= _actionPoints;
        }

        if (m_currentActionsPoints == 0)
        {
            InGameUIManager.Instance.EnableEndRoundButton();
        }
    }

    public override void TakeDamage(float _damageValue)
    {
        m_currentHealth -= _damageValue;

        if(m_currentHealth <= 0.0f)
        {
            m_isAlive = false;
            GameManager.Instance.SetState(GameStates.GAMEOVER);
        }
    }

    public int GetCurrentActionPoints()
    {
        return m_currentActionsPoints;
    }
}
