using System.Collections.Generic;
using _Scripts.Models;

namespace _Scripts.TargetModes
{
    public class AllEnemyTM : TargetMode
    {
        public override List<CombatantView> GetTargets()
        {
            return new List<CombatantView>(EnemySystem.Instance.Enemies);
        }
    }
}