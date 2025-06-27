using System.Collections.Generic;
using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using _Scripts.Models;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Effects
{
    public class DealDamageEffect : Effect
    {
        [SerializeField]
        private int damageAmount;

        public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
        {
            return new DealDamageGA(damageAmount, targets, caster);
        }
    }
}