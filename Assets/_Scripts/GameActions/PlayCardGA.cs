using _Scripts.General.ActionSystem;
using _Scripts.Models;
using _Scripts.Views;
using JetBrains.Annotations;

namespace _Scripts.GameActions
{
    public class PlayCardGA : GameAction
    {
        public PlayCardGA(Card card)
        {
            Card = card;
        }

        public PlayCardGA(Card card, EnemyView target) : this(card)
        {
            Target = target;
        }

        [CanBeNull]
        public EnemyView Target { get; private set; }

        public Card Card { get; private set; }
    }
}