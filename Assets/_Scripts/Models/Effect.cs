using System;
using System.Collections.Generic;
using _Scripts.General.ActionSystem;
using _Scripts.Views;

namespace _Scripts.Models
{
    [Serializable]
    public abstract class Effect
    {
        public abstract GameAction GetGameAction(List<CombatantView> targets, CombatantView caster);
    }
}