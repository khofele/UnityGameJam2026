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
}
