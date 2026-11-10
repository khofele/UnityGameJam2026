using UnityEngine;

public class EntityPlayer : Entity
{
    public bool IsMoving { get; private set; }
    public override void HandleHealthChange()
    {
        if(m_currentHealth <= 0.0f)
        {
            GameManager.Instance.SetState(GameStates.GAMEOVER);
        }
    }
    protected override void OnTick(float fTimeDelta)
    {//oder lieber an/ausschalten bei set SB, OnPause? ---> SB hat kein zugriff auf Entity
        if (m_kinematics.MoveVelo.sqrMagnitude < 0.1f)
            IsMoving = false;
        else
            IsMoving = true;
    }
    public int GetCurrentActionPoints()
    {
        return m_currentActionsPoints;
    }
}
