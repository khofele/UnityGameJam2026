using UnityEngine;

[CreateAssetMenu(fileName = "EntityEnemyBehavior", menuName = "Scriptable Entity Objects/EntityEnemyBehavior")]
public abstract class EntityEnemyBehavior : ScriptableObject
{
    public abstract EntityEnemyDecision MakeActionDecision(EntityEnemy _entityEnemy, Entity _targetEntity);
}
