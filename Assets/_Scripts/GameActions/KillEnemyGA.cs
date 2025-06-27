using _Scripts.General.ActionSystem;
using _Scripts.Views;

namespace _Scripts.GameActions
{
    public class KillEnemyGA : GameAction
    {
        public KillEnemyGA(EnemyView target)
        {
            Target = target;
        }

        public EnemyView Target { get; private set; }
    }
}