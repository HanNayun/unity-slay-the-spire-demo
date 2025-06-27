using System.Collections.Generic;
using _Scripts.General.ActionSystem;
using _Scripts.Interfaces;
using _Scripts.Views;

namespace _Scripts.GameActions
{
    public class DealDamageGA : GameAction, IHaveCaster
    {
        public DealDamageGA(int damage, List<CombatantView> targets, CombatantView caster)
        {
            Damage = damage;
            Targets = targets;
            Caster = caster;
        }

        public int Damage { get; private set; }
        public List<CombatantView> Targets { get; private set; }
        public CombatantView Caster { get; private set; }
    }
}