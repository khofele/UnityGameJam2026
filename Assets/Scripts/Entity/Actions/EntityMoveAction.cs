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
        if (_actionContext.TargetEntity == null)
        {
            return false;
        }

        Vector2 targetPosition = _actionContext.TargetEntity.CurrentNodePos;
        //Node targetNode = GraphManager.Instance.GetNodeAt(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y));
        Node targetNode = GraphManager.Instance.FindClosestNode(_actionContext.TargetEntity.transform.position);

        LevelManager.Instance.CalculateEntityMovePath(_actionContext.ExecutingEntity, targetNode);

        if (LevelManager.Instance.Path == null || LevelManager.Instance.Path.GetSize() < 3)//<3 damit stattdessen angegriffen wird??
        {
            return false;
        }

        int maxPathSize = Mathf.Min(LevelManager.Instance.Path.GetSize() - 1, _actionContext.ExecutingEntity.CurrentActionPoints + 1);
        //LevelManager.Instance.Path.CutPath(maxPathSize);
        LevelManager.Instance.Path.CutPath(2);//darf nur 1 pro action laufen also länge 2

        if (LevelManager.Instance.Path.GetSize() > 1)
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
        LevelManager.Instance.ConfirmEntityMove(_actionContext.ExecutingEntity);
    }
}
