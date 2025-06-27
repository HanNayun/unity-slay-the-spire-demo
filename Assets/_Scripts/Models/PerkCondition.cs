using System;
using UnityEngine;

namespace _Scripts.Models
{
    public abstract class PerkCondition
    {
        [SerializeField]
        protected ReactionTiming reactionTiming;

        public abstract void SubscribeCondition(Action<GameAction> reaction);

        public abstract void UnsubscribeCondition(Action<GameAction> reaction);

        public abstract bool IsSubconditionMet(GameAction gameAction);
    }
}