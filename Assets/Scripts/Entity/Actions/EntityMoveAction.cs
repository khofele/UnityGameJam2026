using UnityEngine;

[CreateAssetMenu(fileName = "EntityMoveAction", menuName = "Scriptable Entity Objects/EntityMoveAction")]
public class EntityMoveAction : EntityAction
{
    [SerializeField] private int m_movedTileAmount = 1;

    public int MovedTileAmount
    {
        get { return m_movedTileAmount; }
    }
}
