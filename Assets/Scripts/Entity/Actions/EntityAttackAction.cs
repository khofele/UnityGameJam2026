using UnityEngine;

[CreateAssetMenu(fileName = "EntityAttackAction", menuName = "Scriptable Entity Objects/EntityAttackAction")]
public class EntityAttackAction : EntityAction
{
    [SerializeField] private float m_damage = 0.0f;
    
    public float Damage
    {
        get { return m_damage; }
    }

    public override bool CanExecuteAction(EntityActionContext _actionContext)
    {
        //int distance = Mathf.Abs(Mathf.RoundToInt(_actionContext.ExecutingEntity.CurrentNodePos.x - _actionContext.TargetEntity.CurrentNodePos.x)) + Mathf.Abs(Mathf.RoundToInt(_actionContext.ExecutingEntity.CurrentNodePos.y - _actionContext.TargetEntity.CurrentNodePos.y));
        float distance = (_actionContext.ExecutingEntity.CurrentNodePos - _actionContext.TargetEntity.CurrentNodePos).magnitude;

        if(distance <= LevelManager.Instance.AttackReach)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteAction(EntityActionContext _actionContext)
    {
        //_actionContext.TargetEntity.TakeDamage(m_damage);//Lenny Pfusch hoffentlich funktionierts
        LevelManager.Instance.ConfirmEntityAttack(_actionContext.ExecutingEntity, _actionContext.TargetEntity, LevelManager.Instance.RollDamage(false));
    }
}
