using _Scripts.General.ActionSystem;

namespace _Scripts.GameActions
{
    public class DrawCardGA : GameAction
    {
        public DrawCardGA(int count)
        {
            Amount = count;
        }

        public int Amount { get; set; }
    }
}