using UnityEngine;

[CreateAssetMenu(fileName = "EntityMoveAction", menuName = "Scriptable Entity Objects/EntityMoveAction")]
public class EntityMoveAction : EntityAction
{
    [SerializeField] private int m_movedTileAmount = 1;

    public int MovedTileAmount
    {
        get { return m_movedTileAmount; }
    }

    public override bool CanExecuteAction(EntityActionContext _actionContext)
    {
        LevelManager.Instance.CalculateEntityMovePath(_actionContext.ExecutingEntity, GraphManager.Instance.FindClosestNode(_actionContext.TargetEntity.currentnodepos));
        // m_path aus Levelmanager --> braucht getter
        // wenn Pfadcount > Aktionspunkte dann false
        // TODO CutPath wenn nicht genug Aktionspunkte und auch sonst keine andere Aktion möglich, dann halt so weit laufen wie geht
        return true;
    }

    public override void ExecuteAction(EntityActionContext _actionContext)
    {
        //TODO ConfirmEntityMove aus LevelManager
    }
}
