using _Scripts.Models;

namespace _Scripts.GameActions
{
    public class PlayCardGA : GameAction
    {
        public Card Card { get; set; }

        public PlayCardGA(Card card)
        {
            Card = card;
        }
    }
}