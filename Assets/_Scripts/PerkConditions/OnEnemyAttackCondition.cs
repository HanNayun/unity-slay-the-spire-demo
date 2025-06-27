using System;
using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using _Scripts.Models;
using _Scripts.Views;

namespace _Scripts.PerkConditions
{
public class OnEnemyAttackCondition : PerkCondition
{
    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackHeroGA>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(reaction, reactionTiming);
    }

    public override bool IsSubconditionMet(GameAction gameAction)
    {
        return gameAction is DealDamageGA { Caster: EnemyView };
    }
}
}