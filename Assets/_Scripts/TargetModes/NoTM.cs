using System.Collections.Generic;
using _Scripts.Models;
using _Scripts.Views;

namespace _Scripts.TargetModes
{
    public class NoTM : TargetMode
    {
        public override List<CombatantView> GetTargets()
        {
            return new List<CombatantView>();
        }
    }
}