using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class EntityPlayer : Entity
{
    private PlayerMovement m_playerMovement;
    private void Start()
    {
        if (TryGetComponent(out PlayerMovement pm))
            m_playerMovement = pm;
    }
    public override void ReduceActionPoints(int _actionPoints)
    {
        if ((m_currentActionsPoints - _actionPoints) >= 0)
        {
            m_currentActionsPoints -= _actionPoints;
        }

        if (m_currentActionsPoints == 0)
        {
            UIManager.Instance.EnableEndRoundButton();
        }
    }

    public override void TakeDamage(float _damageValue)
    {
        m_currentHealth -= _damageValue;

        if (m_currentHealth <= 0.0f)
        {
            m_isAlive = false;
            GameManager.Instance.SetState(GameStates.GAMEOVER);
        }
    }

    protected override void HandleMovementChange(bool _isMoving)
    {
        base.HandleMovementChange(_isMoving);
        UIManager.Instance.SetPlayerMoveButtonInteractable(!_isMoving);
        m_playerMovement.CanClick = !_isMoving;
    }
}
