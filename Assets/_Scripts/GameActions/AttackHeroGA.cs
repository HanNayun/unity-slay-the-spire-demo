using _Scripts.General.ActionSystem;
using _Scripts.Interfaces;
using _Scripts.Views;

namespace _Scripts.GameActions
{
    public class AttackHeroGA : GameAction, IHaveCaster
    {
        public AttackHeroGA(EnemyView attacker)
        {
            Attacker = attacker;
            Caster = Attacker;
        }

        public EnemyView Attacker { get; private set; }
        public CombatantView Caster { get; private set; }
    }
}