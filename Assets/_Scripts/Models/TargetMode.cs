using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models
{
    [System.Serializable]
    public abstract class TargetMode
    {
        public abstract List<CombatantView> GetTargets();
    }
}