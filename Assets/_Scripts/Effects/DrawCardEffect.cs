using System.Collections.Generic;
using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using _Scripts.Models;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Effects
{
    public class DrawCardEffect : Effect
    {
        [SerializeField]
        private int amount;

        public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
        {
            DrawCardGA drawCardGA = new(amount);
            return drawCardGA;
        }
    }
}