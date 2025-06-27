using System.Collections.Generic;
using _Scripts.General.ActionSystem;
using _Scripts.Models;
using _Scripts.Views;

namespace _Scripts.GameActions
{
    public class PerformEffectGA : GameAction
    {
        public PerformEffectGA(Effect effect, List<CombatantView> targets)
        {
            Effect = effect;
            Targets = targets;
        }

        public Effect Effect { get; private set; }
        public List<CombatantView> Targets { get; private set; }
    }
}