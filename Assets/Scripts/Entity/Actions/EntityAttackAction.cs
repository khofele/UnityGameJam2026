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
        //if (_actionContext.TargetEntity == null)
        //{
        //    return false;
        //}

        //int tileDistance = _actionContext.ExecutingEntity.GetTileDistance(_actionContext.TargetEntity);

        //if (tileDistance == 1)
        //{
        //    return true;
        //}
        //else
        //{
            return true;
        //TODO}
    }

    public override void ExecuteAction(EntityActionContext _actionContext)
    {
        _actionContext.TargetEntity.TakeDamage(m_damage);
    }
}
