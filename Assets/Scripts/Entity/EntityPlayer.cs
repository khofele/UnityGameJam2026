using UnityEngine;

public class EntityPlayer : Entity
{
    
    public override void HandleHealthChange()
    {
        if(m_currentHealth <= 0.0f)
        {
            GameManager.Instance.SetState(GameStates.GAMEOVER);
        }
    }
    public int GetCurrentActionPoints()
    {
        return m_currentActionsPoints;
    }
    protected override void HandleMovementChange(bool _isMoving)
    {
        base.HandleMovementChange(_isMoving);
        UIManager.Instance.SetPlayerMoveButtonInteractable(!_isMoving);
    }
}
