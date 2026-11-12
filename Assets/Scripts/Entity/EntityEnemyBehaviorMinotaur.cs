using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityEnemyBehaviorMinotaur", menuName = "Scriptable Entity Objects/EntityEnemyBehaviorMinotaur")]
public class EntityEnemyBehaviorMinotaur : EntityEnemyBehavior
{
    public override EntityEnemyDecision MakeActionDecision(EntityEnemy _entityEnemy, Entity _targetEntity)
    {
        if (_targetEntity == null)
        {
            return null;
        }

        List<EntityEnemyDecision> availableDecisions = new List<EntityEnemyDecision>();

        foreach (EntityAction action in _entityEnemy.EntityProperties.EntityActions)
        {
            EntityMoveAction moveAction = null;

            if (action is EntityAttackAction)
            {
                EntityActionContext actionContext = new EntityActionContext(_entityEnemy, _targetEntity);

                if (_entityEnemy.CanExecuteAction(action, actionContext) == true)
                {
                    availableDecisions.Add(new EntityEnemyDecision(action, _targetEntity));
                }
            }
            else if (action is EntityMoveAction)
            {
                moveAction = (EntityMoveAction)action;

                EntityEnemyDecision decision = new EntityEnemyDecision(action, _targetEntity);
                EntityActionContext actionContext = new EntityActionContext(_entityEnemy, _targetEntity);

                if(_entityEnemy.CanExecuteAction(action, actionContext) == true) 
                {
                    availableDecisions.Add(decision);
                }
            }
        }

        if (availableDecisions.Count == 0)
        {
            return null;
        }

        EntityEnemyDecision resultDecision = availableDecisions[0];

        foreach (EntityEnemyDecision decision in availableDecisions)
        {
            if (decision.ChosenAction.ActionPriority > resultDecision.ChosenAction.ActionPriority)
            {
                resultDecision = decision;
            }
        }

        return resultDecision;
    }
}