using System;
using _Scripts.Models;

namespace _Scripts.PerkConditions
{
    public class OnEenemyAttackCondition : PerkCondition
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
            return true;
        }
    }
}