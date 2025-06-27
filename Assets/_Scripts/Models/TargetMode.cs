using System;
using System.Collections.Generic;

namespace _Scripts.Models
{
    [Serializable]
    public abstract class TargetMode
    {
        public abstract List<CombatantView> GetTargets();
    }
}