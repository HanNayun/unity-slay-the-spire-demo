using System;
using System.Collections.Generic;
using _Scripts.Views;

namespace _Scripts.Models
{
    [Serializable]
    public abstract class TargetMode
    {
        public abstract List<CombatantView> GetTargets();
    }
}