using System;
using System.Collections.Generic;

namespace _Scripts.Models
{
    [Serializable]
    public abstract class Effect
    {
        public abstract GameAction GetGameAction(List<CombatantView> targets);
    }
}