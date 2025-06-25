using System.Collections.Generic;
using _Scripts.Models;

namespace _Scripts.GameActions
{
    public class PerformEffectGA : GameAction
    {
        public Effect Effect { get; private set; }
        public List<CombatantView> Targets { get; private set; }

        public PerformEffectGA(Effect effect, List<CombatantView> targets)
        {
            Effect = effect;
            Targets = targets;
        }
    }
}